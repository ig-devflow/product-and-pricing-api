using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class AccommodationTypeConfiguration : IEntityTypeConfiguration<AccommodationType>
{
    public void Configure(EntityTypeBuilder<AccommodationType> entity)
    {
        entity.ToTable("AccommodationType", "Product");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(AccommodationType.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.Prefix)
            .HasConversion<string>()
            .HasMaxLength(AccommodationType.Rules.PrefixMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}