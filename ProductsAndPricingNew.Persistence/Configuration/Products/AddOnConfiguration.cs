using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class AddOnConfiguration : IEntityTypeConfiguration<AddOn>
{
    public void Configure(EntityTypeBuilder<AddOn> entity)
    {
        entity.ToTable("AddOn", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.DivisionId).IsRequired();
        entity.Property(x => x.Name).HasMaxLength(AddOn.Rules.NameMaxLength).IsRequired();
        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.Type).HasColumnName("AddOnTypeId").HasConversion<int>().IsRequired();
        entity.Property(x => x.UnitTypeId).IsRequired();
        entity.Property(x => x.OneToOneLessonsPerWeek);

        entity.ConfigureProductCategories(x => x.Categories);
        entity.ConfigureAgeRange(x => x.AgeRange);
        entity.ConfigureFinanceCodes(x => x.FinanceCodes);

        entity.Property(x => x.ClosurePolicy)
            .HasConversion(Converters.OfferingsClosurePolicy)
            .HasColumnName("OfferingsClosureDate");

        entity.HasIndex(x => new { x.DivisionId, x.Name });
        entity.HasIndex(x => x.Type);

        entity.HasOne<Division>()
            .WithMany()
            .HasForeignKey(x => x.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ConfigureAuditAndConcurrency();
    }
}