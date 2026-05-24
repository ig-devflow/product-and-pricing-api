using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.CancellationFees;

namespace ProductsAndPricingNew.Persistence.Configuration.FeeOrDiscount;

internal sealed class CancellationFeeOfferingConfiguration : IEntityTypeConfiguration<CancellationFeeOffering>
{
    public void Configure(EntityTypeBuilder<CancellationFeeOffering> b)
    {
        b.ToTable("CancellationFeeOffering", "FeeOrDiscount");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.SchoolId).IsRequired();
        b.Property(x => x.CancellationFeeId).IsRequired();

        b.ComplexProperty(x => x.Years, y =>
        {
            y.Property(v => v.From).HasColumnName("ActiveFromPricingYear").IsRequired();
            y.Property(v => v.To).HasColumnName("ActiveToPricingYear");
        });

        b.ComplexProperty(x => x.ApplyRuleset, r =>
        {
            r.Property<int>("RulesetId").HasColumnName("ApplyRulesetId");
        });

        b.ComplexProperty(x => x.ChargeRuleset, r =>
        {
            r.Property<int>("RulesetId").HasColumnName("ChargeRulesetId");
        });

        b.HasIndex(x => new { x.SchoolId, x.CancellationFeeId });
        b.HasOne<CancellationFee>().WithMany().HasForeignKey(x => x.CancellationFeeId).OnDelete(DeleteBehavior.Restrict);

        b.ConfigureAuditAndConcurrency();
    }
}