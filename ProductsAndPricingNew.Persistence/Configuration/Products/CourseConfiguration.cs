using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> entity)
    {
        entity.ToTable("Course", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name).HasMaxLength(Course.Rules.NameMaxLength).IsRequired();
        entity.Property(x => x.DivisionId).IsRequired();
        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.CourseLanguageId).IsRequired();
        entity.Property(x => x.CourseIntensityId).IsRequired();
        entity.Property(x => x.UnitTypeId).IsRequired();
        entity.Property(x => x.MinimumAge);
        entity.Property(x => x.MinimumWeeks);
        entity.Property(x => x.AccountCategoryId);
        entity.Property(x => x.ProductCategoryId);
        entity.Property(x => x.OfferingsClosureDate);

        entity.ConfigureFinanceCodes(x => x.FinanceCodes);

        entity.HasIndex(x => new { x.DivisionId, x.Name });

        entity.ConfigureAuditMetadata(x => x.AuditMetadata);
        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.Version).IsRowVersion();

        entity.Ignore(x => x.DomainEvents);
    }
}