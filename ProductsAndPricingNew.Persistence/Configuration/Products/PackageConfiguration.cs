using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> entity)
    {
        entity.ToTable("Package", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.DivisionId).IsRequired();
        entity.Property(x => x.UnitTypeId).IsRequired();

        entity.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(x => x.IsActive).IsRequired();

        entity.Property(x => x.Description)
            .HasMaxLength(4000);

        entity.Property(x => x.CommissionPercentage)
            .HasPrecision(5, 2)
            .IsRequired();

        entity.Property(x => x.MinimumAge);
        entity.Property(x => x.MaximumAge);
        entity.Property(x => x.MinimumWeeks);

        entity.Property(x => x.AccountCategoryId);
        entity.Property(x => x.ProductCategoryId);
        entity.Property(x => x.OfferingsClosureDate);

        entity.ComplexProperty(x => x.FinanceCodes, finance =>
        {
            finance.Property(x => x.GeneralLedgerCode)
                .HasColumnName("GeneralLedgerCode")
                .HasMaxLength(10);

            finance.Property(x => x.CostCentreCode)
                .HasColumnName("CostCentreCode")
                .HasMaxLength(10);
        });

        entity.HasIndex(x => new { x.DivisionId, x.Name });
        entity.HasIndex(x => x.AccountCategoryId);
        entity.HasIndex(x => x.ProductCategoryId);
        entity.HasIndex(x => x.UnitTypeId);

        entity.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        entity.OwnsMany(x => x.Items, owned =>
        {
            owned.ToTable("PackageItem");

            owned.WithOwner().HasForeignKey("PackageId");

            owned.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            owned.HasKey(x => x.Id);

            owned.Property(x => x.ProductKind)
                .HasColumnName("ProductKind")
                .HasConversion<int>()
                .IsRequired();

            owned.Property(x => x.ProductDefinitionId)
                .HasColumnName("ProductDefinitionId")
                .IsRequired();

            owned.Property(x => x.Percentage)
                .HasColumnName("Percentage")
                .HasPrecision(5, 2)
                .IsRequired();

            owned.Ignore(x => x.Product);

            owned.HasIndex("PackageId", nameof(PackageItem.ProductKind), nameof(PackageItem.ProductDefinitionId))
                .IsUnique();
        });

        entity.ConfigureAuditMetadata(x => x.AuditMetadata);
        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.Version).IsRowVersion();

        entity.Ignore(x => x.DomainEvents);
    }
}