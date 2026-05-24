using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

namespace ProductsAndPricingNew.Persistence.Configuration.FeeOrDiscount;

/// <summary>
/// Base mapping for the fee-offering hierarchy. One-off and supplement offerings share this table
/// (table-per-hierarchy), discriminated by FeeOfferingTypeId.
/// </summary>
internal sealed class FeeOfferingConfiguration : IEntityTypeConfiguration<FeeOffering>
{
    public void Configure(EntityTypeBuilder<FeeOffering> b)
    {
        b.ToTable("FeeOffering", "FeeOrDiscount");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.SchoolId).IsRequired();
        b.Property(x => x.FeeId).IsRequired();

        b.Ignore(x => x.Kind);
        b.HasDiscriminator<int>("FeeOfferingTypeId")
            .HasValue<OneOffFeeOffering>((int)FeePricingKind.OneOff)
            .HasValue<SupplementFeeOffering>((int)FeePricingKind.Supplement);

        b.ComplexProperty(x => x.Years, y =>
        {
            y.Property(v => v.From).HasColumnName("ActiveFromPricingYear").IsRequired();
            y.Property(v => v.To).HasColumnName("ActiveToPricingYear");
        });

        b.ComplexProperty(x => x.ApplyRuleset, r =>
        {
            r.Property<int>("RulesetId").HasColumnName("ApplyRulesetId");
        });

        b.ComplexProperty(x => x.ApplicableRowRuleset, r =>
        {
            r.Property<int>("RulesetId").HasColumnName("ApplicableRowRulesetId");
        });

        b.HasIndex(x => new { x.SchoolId, x.FeeId });
        b.HasOne<Fee>().WithMany().HasForeignKey(x => x.FeeId).OnDelete(DeleteBehavior.Restrict);

        b.ConfigureAuditAndConcurrency();
    }
}