using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> entity)
    {
        entity.ToTable("Course", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.DivisionId).IsRequired();
        entity.Property(x => x.Name).HasMaxLength(Course.Rules.NameMaxLength).IsRequired();
        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.CourseLanguageId).IsRequired();
        entity.Property(x => x.CourseIntensityId).IsRequired();
        entity.Property(x => x.UnitTypeId).IsRequired();
        entity.Property(x => x.MinimumWeeks);

        entity.ConfigureProductCategories(x => x.Categories);
        entity.ConfigureAgeRange(x => x.AgeRange);
        entity.ConfigureFinanceCodes(x => x.FinanceCodes);

        entity.Property(x => x.ClosurePolicy)
            .HasConversion(Converters.OfferingsClosurePolicy)
            .HasColumnName("OfferingsClosureDate");

        entity.HasIndex(x => new { x.DivisionId, x.Name });

        entity.HasOne<Division>()
            .WithMany()
            .HasForeignKey(x => x.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ConfigureAuditAndConcurrency();
    }
}
