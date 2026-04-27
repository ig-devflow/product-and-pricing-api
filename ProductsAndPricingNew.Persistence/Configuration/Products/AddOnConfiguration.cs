using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class AddOnConfiguration : IEntityTypeConfiguration<AddOn>
{
    public void Configure(EntityTypeBuilder<AddOn> entity)
    {
        entity.ToTable("AddOn", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.DivisionId).IsRequired();
        entity.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.AddOnTypeId).IsRequired();
        entity.Property(x => x.UnitTypeId).IsRequired();

        entity.Property(x => x.MinimumAge);
        entity.Property(x => x.AccountCategoryId);
        entity.Property(x => x.ProductCategoryId);

        entity.Property(x => x.OneToOneLessonsPerWeek)
            .HasColumnName("OneToOneLessonsPerWeek");

        entity.Property(x => x.OfferingsClosureDate);

        entity.ComplexProperty(x => x.FinanceCodes, complex =>
        {
            complex.Property(p => p.GeneralLedgerCode)
                .HasColumnName("GeneralLedgerCode")
                .HasMaxLength(10);

            complex.Property(p => p.CostCentreCode)
                .HasColumnName("CostCentreCode")
                .HasMaxLength(10);
        });

        entity.HasIndex(x => new { x.DivisionId, x.Name });
        entity.HasIndex(x => x.AccountCategoryId);
        entity.HasIndex(x => x.ProductCategoryId);
        entity.HasIndex(x => x.AddOnTypeId);
        entity.HasIndex(x => x.UnitTypeId);

        entity.ConfigureAuditMetadata(x => x.AuditMetadata);
        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.Version).IsRowVersion();

        entity.Ignore(x => x.DomainEvents);
    }
}