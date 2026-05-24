using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

namespace ProductsAndPricingNew.Persistence.Configuration.FeeOrDiscount;

internal sealed class SupplementFeeOfferingConfiguration : IEntityTypeConfiguration<SupplementFeeOffering>
{
    public void Configure(EntityTypeBuilder<SupplementFeeOffering> b)
    {
        b.Ignore(x => x.Kind);

        b.Property(x => x.HasSpecificDates).IsRequired();
        b.Property(x => x.MaximumUnits);

        b.OwnsMany(x => x.Prices, price =>
        {
            price.ToTable("SupplementPrice", "FeeOrDiscount");
            price.WithOwner().HasForeignKey("FeeOfferingId");
            price.Property(x => x.Id).ValueGeneratedOnAdd();
            price.HasKey(x => x.Id);

            price.Property(x => x.Year).IsRequired();
            price.Property(x => x.CurrencyId).IsRequired();
            price.Property(x => x.PricePerMajorUnit).HasPrecision(18, 2).IsRequired();
            price.Property(x => x.PricePerMinorUnit).HasPrecision(18, 2).IsRequired();
            price.Property(x => x.PeriodStart);
            price.Property(x => x.PeriodEnd);

            price.Ignore(x => x.IsAllDates);
        })
        .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}