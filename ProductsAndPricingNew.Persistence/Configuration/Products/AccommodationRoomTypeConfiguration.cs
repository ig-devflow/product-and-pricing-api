using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class AccommodationRoomTypeConfiguration : IEntityTypeConfiguration<AccommodationRoomType>
{
    public void Configure(EntityTypeBuilder<AccommodationRoomType> entity)
    {
        entity.ToTable("AccommodationRoomType", "Product");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(AccommodationRoomType.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}