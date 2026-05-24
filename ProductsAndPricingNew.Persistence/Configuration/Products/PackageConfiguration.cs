using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
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
        entity.Property(x => x.Name).HasMaxLength(Package.Rules.NameMaxLength).IsRequired();
        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.Description).HasMaxLength(4000);
        entity.Property(x => x.MinimumWeeks);

        entity.ComplexProperty(x => x.Commission, c =>
        {
            c.Property(v => v.Value).HasColumnName("CommissionPercentage").HasPrecision(5, 2).IsRequired();
        });

        entity.ConfigureProductCategories(x => x.Categories);
        entity.ConfigureAgeRange(x => x.AgeRange);
        entity.ConfigureFinanceCodes(x => x.FinanceCodes);

        entity.Property(x => x.ClosurePolicy)
            .HasConversion(Converters.OfferingsClosurePolicy)
            .HasColumnName("OfferingsClosureDate");

        entity.HasIndex(x => new { x.DivisionId, x.Name });

        entity.HasOne<Division>()
            .WithMany()
            .HasForeignKey(x => x.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.OwnsMany(x => x.Items, owned =>
        {
            owned.ToTable("PackageItem", "Product");
            owned.WithOwner().HasForeignKey("PackageId");
            owned.Property<int>("Id").ValueGeneratedOnAdd();
            owned.HasKey("Id");

            owned.Property(x => x.ProductKind)
                .HasColumnName("ProductKind")
                .HasConversion<int>()
                .IsRequired();

            owned.Property(x => x.ProductId)
                .HasColumnName("ProductDefinitionId")
                .IsRequired();

            owned.Property(x => x.Percentage)
                .HasColumnName("Percentage")
                .HasConversion(Converters.Percentage)
                .HasPrecision(5, 2)
                .IsRequired();

            owned.Ignore(x => x.Product);

            owned.HasIndex("PackageId", nameof(PackageItem.ProductKind), nameof(PackageItem.ProductId))
                .IsUnique();
        })
        .UsePropertyAccessMode(PropertyAccessMode.Field);

        entity.ConfigureAuditAndConcurrency();
    }
}
