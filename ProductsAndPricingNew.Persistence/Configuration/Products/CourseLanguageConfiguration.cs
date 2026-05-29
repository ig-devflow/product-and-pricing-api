using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class CourseLanguageConfiguration : IEntityTypeConfiguration<CourseLanguage>
{
    public void Configure(EntityTypeBuilder<CourseLanguage> entity)
    {
        entity.ToTable("CourseLanguage", "Product");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(CourseLanguage.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        entity.HasData(
            new { Id = 1, Name = "English", IsDeleted = false },
            new { Id = 2, Name = "French", IsDeleted = false },
            new { Id = 3, Name = "Bilingual", IsDeleted = false },
            new { Id = 4, Name = "Maltese", IsDeleted = false }
        );
    }
}
