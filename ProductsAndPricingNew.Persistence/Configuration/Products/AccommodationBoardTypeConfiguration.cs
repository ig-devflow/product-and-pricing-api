using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class AccommodationBoardTypeConfiguration : IEntityTypeConfiguration<AccommodationBoardType>
{
    public void Configure(EntityTypeBuilder<AccommodationBoardType> entity)
    {
        entity.ToTable("AccommodationBoardType", "Product");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(AccommodationBoardType.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}