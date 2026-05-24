using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class PricingYearConfiguration : IEntityTypeConfiguration<PricingYear>
{
    public void Configure(EntityTypeBuilder<PricingYear> entity)
    {
        entity.ToTable("PricingYear", "Pricing");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.DivisionId).IsRequired();
        entity.Property(x => x.Year).IsRequired();

        entity.ComplexProperty(x => x.EarlyBirdRuleset, r =>
        {
            r.Property<int>("RulesetId").HasColumnName("EarlyBirdRulesetId");
        });

        entity.HasIndex(x => new { x.DivisionId, x.Year }).IsUnique();

        entity.ConfigureAuditAndConcurrency();
    }
}