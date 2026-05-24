using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Tax;

namespace ProductsAndPricingNew.Persistence.Configuration.Tax;

internal sealed class TaxBandConfiguration : IEntityTypeConfiguration<TaxBand>
{
    public void Configure(EntityTypeBuilder<TaxBand> b)
    {
        b.ToTable("TaxBand", "Tax");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.TaxRegimenId).IsRequired();
        b.Property(x => x.Name).HasMaxLength(TaxBand.Rules.NameMaxLength).IsRequired();
        b.Property(x => x.TaxCode).HasMaxLength(TaxBand.Rules.TaxCodeMaxLength).IsRequired();
        b.Property(x => x.Rate).HasPrecision(9, 6).IsRequired();
        b.Property(x => x.SequenceKey).IsRequired();

        b.ComplexProperty(x => x.BandRuleset, r =>
        {
            r.Property<int>("RulesetId").HasColumnName("BandRulesetId");
        });

        b.HasIndex(x => new { x.TaxRegimenId, x.SequenceKey });
        b.HasIndex(x => new { x.TaxRegimenId, x.TaxCode }).IsUnique();
    }
}
