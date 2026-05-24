using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Persistence.Configuration.Rules;

internal sealed class PricingRuleConfiguration : IEntityTypeConfiguration<PricingRule>
{
    public void Configure(EntityTypeBuilder<PricingRule> b)
    {
        b.ToTable("PricingRule", "Rules");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.RulesetId).IsRequired();
        b.Property(x => x.Name).HasMaxLength(PricingRule.Rules.NameMaxLength).IsRequired();
        b.Property(x => x.ScriptJson).HasColumnType("nvarchar(max)").IsRequired();
        b.Property(x => x.Priority).IsRequired();
        b.Property(x => x.IsActive).IsRequired();

        b.HasIndex(x => new { x.RulesetId, x.Priority });
        b.HasIndex(x => new { x.RulesetId, x.Name }).IsUnique();
    }
}
