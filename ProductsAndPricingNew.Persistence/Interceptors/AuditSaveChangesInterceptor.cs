using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    private readonly ISystemClock _clock;

    public AuditSaveChangesInterceptor(ICurrentUser currentUser, ISystemClock clock)
    {
        _currentUser = currentUser;
        _clock = clock;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudit(DbContext? context)
    {
        if (context is null)
            return;

        var now = _clock.UtcNow;
        var userId = _currentUser.UserId;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is not IAggregateRoot)
                continue;

            if (entry.Metadata.FindProperty(nameof(AggregateRoot<int>.EditInfo)) is null)
                continue;

            var current = (EditInfo?)entry.Property(nameof(AggregateRoot<int>.EditInfo)).CurrentValue;

            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(AggregateRoot<int>.EditInfo)).CurrentValue =
                    EditInfo.Create(userId, now);
            }
            else if (entry.State == EntityState.Modified && current is not null)
            {
                entry.Property(nameof(AggregateRoot<int>.EditInfo)).CurrentValue =
                    current.Touch(userId, now);
            }
        }
    }
}