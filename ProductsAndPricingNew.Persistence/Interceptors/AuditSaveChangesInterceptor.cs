using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Persistence.Interceptors;

internal sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
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

        DateTimeOffset now = _clock.UtcNow;
        int userId = _currentUser.UserId;

        foreach (EntityEntry<IHasAuditMetadata> entry in context.ChangeTracker.Entries<IHasAuditMetadata>())
        {
            ComplexPropertyEntry<IHasAuditMetadata, AuditMetadata> audit = entry.ComplexProperty<AuditMetadata>(nameof(IHasAuditMetadata.AuditMetadata));

            if (entry.State == EntityState.Added)
            {
                audit.CurrentValue = AuditMetadata.Create(userId, now);
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                audit.CurrentValue = audit.CurrentValue.MarkUpdated(userId, now);
            }
        }
    }
}