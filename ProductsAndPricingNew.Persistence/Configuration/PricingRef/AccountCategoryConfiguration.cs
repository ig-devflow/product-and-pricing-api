using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

public sealed class AccountCategoryConfiguration : IEntityTypeConfiguration<AccountCategory>
{
    public void Configure(EntityTypeBuilder<AccountCategory> b)
    {
        b.ToTable("AccountCategory");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.DivisionId).IsRequired();

        b.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.IsActive).IsRequired();
        b.Property(x => x.IsDeleted).IsRequired();

        b.HasIndex(x => new { x.DivisionId, x.Name });
        b.HasIndex(x => new { x.DivisionId, x.IsDeleted, x.IsActive });

        b.ConfigureAuditMetadata(x => x.AuditMetadata);
        b.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        b.Property(x => x.RowVersion).IsRowVersion();

        b.Ignore(x => x.DomainEvents);
    }
}