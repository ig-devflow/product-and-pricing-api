using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentreContactTypeEntity = ProductsAndPricingNew.Domain.ReferenceData.CentreContactType;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class CentreContactTypeConfiguration : IEntityTypeConfiguration<CentreContactTypeEntity>
{
    public void Configure(EntityTypeBuilder<CentreContactTypeEntity> entity)
    {
        entity.ToTable("CentreContactType", "PricingRef");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(CentreContactTypeEntity.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}