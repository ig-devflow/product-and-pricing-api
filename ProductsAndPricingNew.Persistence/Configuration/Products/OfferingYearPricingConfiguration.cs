using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Pricing;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class OfferingYearPricingConfiguration : IEntityTypeConfiguration<OfferingYearPricing>
{
    public void Configure(EntityTypeBuilder<OfferingYearPricing> entity)
    {
        entity.ToTable("OfferingYearPricing", "Pricing");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.ProductOfferingId).IsRequired();
        entity.Property(x => x.Year).IsRequired();

        entity.HasIndex(x => new { x.ProductOfferingId, x.Year }).IsUnique();
        entity.HasOne<Domain.Entities.Offerings.ProductOffering>()
            .WithMany()
            .HasForeignKey(x => x.ProductOfferingId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.OwnsMany(x => x.Periods, period =>
        {
            period.ToTable("PricePeriod", "Pricing");
            period.WithOwner().HasForeignKey("OfferingYearPricingId");
            period.Property<int>("Id").ValueGeneratedOnAdd();
            period.HasKey("Id");

            period.Property(x => x.Start).IsRequired();
            period.Property(x => x.End).IsRequired();
            period.Property(x => x.CurrencyId).IsRequired();
            period.Property(x => x.BandingMethod).HasConversion<int>().IsRequired();

            period.Ignore(x => x.Range);

            period.OwnsMany(x => x.Bands, band =>
            {
                band.ToTable("PriceBand", "Pricing");
                band.WithOwner().HasForeignKey("PricePeriodId");
                band.Property<int>("Id").ValueGeneratedOnAdd();
                band.HasKey("Id");

                band.Property(x => x.MinUnits).IsRequired();
                band.Property(x => x.MaxUnits).IsRequired();
                band.Property(x => x.PricePerUnit).HasPrecision(18, 2).IsRequired();
            })
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        })
        .UsePropertyAccessMode(PropertyAccessMode.Field);

        entity.ConfigureAuditAndConcurrency();
    }
}
