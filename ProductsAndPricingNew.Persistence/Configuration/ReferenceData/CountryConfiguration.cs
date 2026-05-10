using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.ReferenceData;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> entity)
    {
        entity.ToTable("Country", "ReferenceData");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Code)
            .HasMaxLength(Country.Rules.CodeMaxLength)
            .IsRequired();

        entity.Property(x => x.Name)
            .HasMaxLength(Country.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}