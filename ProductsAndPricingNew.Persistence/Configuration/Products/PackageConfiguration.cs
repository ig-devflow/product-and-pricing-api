using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

public sealed class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> b)
    {
        b.ToTable("Package");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.DivisionId).IsRequired();
        b.Property(x => x.UnitTypeId).IsRequired();

        b.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        b.Property(x => x.IsActive).IsRequired();

        b.Property(x => x.Description)
            .HasMaxLength(4000);

        b.Property(x => x.CommissionPercentage)
            .HasPrecision(5, 2)
            .IsRequired();

        b.Property(x => x.MinimumAge);
        b.Property(x => x.MaximumAge);
        b.Property(x => x.MinimumWeeks);

        b.Property(x => x.AccountCategoryId);
        b.Property(x => x.ProductCategoryId);
        b.Property(x => x.OfferingsClosureDate);

        b.ComplexProperty(x => x.FinanceCodes, finance =>
        {
            finance.Property(x => x.GeneralLedgerCode)
                .HasColumnName("GeneralLedgerCode")
                .HasMaxLength(50);

            finance.Property(x => x.CostCentreCode)
                .HasColumnName("CostCentreCode")
                .HasMaxLength(50);
        });

        b.HasIndex(x => new { x.DivisionId, x.Name });
        b.HasIndex(x => x.AccountCategoryId);
        b.HasIndex(x => x.ProductCategoryId);
        b.HasIndex(x => x.UnitTypeId);

        b.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        b.OwnsMany(x => x.Items, owned =>
        {
            owned.ToTable("PackageItem");

            owned.WithOwner().HasForeignKey("PackageId");

            owned.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            owned.HasKey(x => x.Id);

            owned.Property(x => x.ProductKind)
                .HasColumnName("ProductKindId")
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

            owned.HasIndex("PackageId", "ProductKindId", "ProductDefinitionId")
                .IsUnique();
        });

        b.ComplexProperty(x => x.EditInfo, audit =>
        {
            audit.Property(x => x.CreatedBy)
                .HasColumnName("CreatedBy")
                .IsRequired();

            audit.Property(x => x.CreateTimestamp)
                .HasColumnName("CreateTimestamp")
                .IsRequired();

            audit.Property(x => x.UpdatedBy)
                .HasColumnName("UpdatedBy")
                .IsRequired();

            audit.Property(x => x.UpdateTimestamp)
                .HasColumnName("UpdateTimestamp")
                .IsRequired();
        });
        
        b.Property(x => x.RowVersion).IsRowVersion();
        
        b.Ignore(x => x.DomainEvents);
    }
}