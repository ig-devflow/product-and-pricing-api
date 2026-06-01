using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class CourseIntensityConfiguration : IEntityTypeConfiguration<CourseIntensity>
{
    public void Configure(EntityTypeBuilder<CourseIntensity> entity)
    {
        entity.ToTable("CourseIntensity", "Product");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(CourseIntensity.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        entity.HasData(
            new { Id = 1, Name = "Standard", IsDeleted = false },
            new { Id = 2, Name = "Semi Intensive", IsDeleted = false },
            new { Id = 3, Name = "Intensive", IsDeleted = false },
            new { Id = 4, Name = "ElectiveOnly", IsDeleted = false },
            new { Id = 5, Name = "Pre Intensive", IsDeleted = false },
            new { Id = 6, Name = "Fast Track Standard", IsDeleted = false },
            new { Id = 7, Name = "Fast Track Intensive", IsDeleted = false }
        );
    }
}
