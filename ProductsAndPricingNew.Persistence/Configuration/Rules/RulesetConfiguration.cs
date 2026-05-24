using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Persistence.Configuration.Rules;

internal sealed class RulesetConfiguration : IEntityTypeConfiguration<Ruleset>
{
    public void Configure(EntityTypeBuilder<Ruleset> b)
    {
        b.ToTable("Ruleset", "Rules");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.Name).HasMaxLength(Ruleset.EntityRules.NameMaxLength).IsRequired();
        b.Property(x => x.Description).HasMaxLength(Ruleset.EntityRules.DescriptionMaxLength);
        b.Property(x => x.SubjectType).HasConversion<int>().IsRequired();
        b.Property(x => x.Status).HasConversion<int>().IsRequired();
        b.Property(x => x.DivisionId).IsRequired();
        b.Property(x => x.RuleVersion).IsRequired();

        // The relationship is mapped through the backing field; the read-only Rules
        // property must not be discovered as a second navigation over the same field.
        b.Ignore(x => x.Rules);

        b.HasMany<PricingRule>("_rules")
            .WithOne()
            .HasForeignKey(x => x.RulesetId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Navigation("_rules")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        b.HasIndex(x => new { x.DivisionId, x.SubjectType, x.Status });

        b.ConfigureAuditAndConcurrency();
    }
}
