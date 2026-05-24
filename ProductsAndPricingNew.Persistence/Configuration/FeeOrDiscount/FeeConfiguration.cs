using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

namespace ProductsAndPricingNew.Persistence.Configuration.FeeOrDiscount;

internal sealed class FeeConfiguration : IEntityTypeConfiguration<Fee>
{
    public void Configure(EntityTypeBuilder<Fee> b)
    {
        b.ToTable("Fee", "FeeOrDiscount");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.Name).HasMaxLength(Fee.Rules.NameMaxLength).IsRequired();
        b.Property(x => x.UnitTypeId).IsRequired();
        b.Property(x => x.DivisionId).IsRequired();
        b.Property(x => x.IsActive).IsRequired();

        b.ConfigureFinanceCodes(x => x.FinanceCodes);

        b.HasIndex(x => new { x.DivisionId, x.Name });

        b.ConfigureAuditAndConcurrency();
    }
}
