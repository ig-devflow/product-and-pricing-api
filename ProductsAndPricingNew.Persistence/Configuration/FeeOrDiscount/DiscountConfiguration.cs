using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Discounts;

namespace ProductsAndPricingNew.Persistence.Configuration.FeeOrDiscount;

internal sealed class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> b)
    {
        b.ToTable("Discount", "FeeOrDiscount");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.Name).HasMaxLength(Discount.Rules.NameMaxLength).IsRequired();
        b.Property(x => x.Description).HasMaxLength(Discount.Rules.DescriptionMaxLength);
        b.Property(x => x.DivisionId).IsRequired();
        b.Property(x => x.Type).HasConversion<int>().IsRequired();
        b.Property(x => x.IsActive).IsRequired();

        b.ComplexProperty(x => x.ShouldApplyRuleset, r =>
        {
            r.Property<int>("RulesetId").HasColumnName("ShouldApplyRulesetId");
        });

        b.HasIndex(x => new { x.DivisionId, x.Type });

        b.ConfigureAuditAndConcurrency();
    }
}
