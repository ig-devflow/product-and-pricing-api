using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.CancellationFees;

namespace ProductsAndPricingNew.Persistence.Configuration.FeeOrDiscount;

internal sealed class CancellationFeeConfiguration : IEntityTypeConfiguration<CancellationFee>
{
    public void Configure(EntityTypeBuilder<CancellationFee> b)
    {
        b.ToTable("CancellationFee", "FeeOrDiscount");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.Name).HasMaxLength(CancellationFee.Rules.NameMaxLength).IsRequired();
        b.Property(x => x.DivisionId).IsRequired();
        b.Property(x => x.IsActive).IsRequired();

        b.ConfigureProductCategories(x => x.Categories);

        b.HasIndex(x => new { x.DivisionId, x.Name });

        b.ConfigureAuditAndConcurrency();
    }
}