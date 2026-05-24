using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

namespace ProductsAndPricingNew.Persistence.Configuration.FeeOrDiscount;

internal sealed class OneOffFeeOfferingConfiguration : IEntityTypeConfiguration<OneOffFeeOffering>
{
    public void Configure(EntityTypeBuilder<OneOffFeeOffering> b)
    {
        b.Ignore(x => x.Kind);

        b.OwnsMany(x => x.Prices, price =>
        {
            price.ToTable("OneOffFeePrice", "FeeOrDiscount");
            price.WithOwner().HasForeignKey("FeeOfferingId");
            price.Property(x => x.Id).ValueGeneratedOnAdd();
            price.HasKey(x => x.Id);

            price.Property(x => x.Year).IsRequired();
            price.Property(x => x.CurrencyId).IsRequired();
            price.Property(x => x.Amount).HasPrecision(18, 2).IsRequired();

            price.HasIndex("FeeOfferingId", nameof(OneOffFeePrice.Year), nameof(OneOffFeePrice.CurrencyId)).IsUnique();
        })
        .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}