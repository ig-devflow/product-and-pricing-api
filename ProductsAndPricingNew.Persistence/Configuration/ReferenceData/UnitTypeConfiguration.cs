using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Persistence.Configuration.ReferenceData;

internal sealed class UnitTypeConfiguration : IEntityTypeConfiguration<UnitType>
{
    public void Configure(EntityTypeBuilder<UnitType> entity)
    {
        entity.ToTable("UnitType", "ReferenceData");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name).HasMaxLength(UnitType.Rules.NameMaxLength).IsRequired();
        entity.Property(x => x.Description).HasMaxLength(UnitType.Rules.DescriptionMaxLength).IsRequired();
        entity.Property(x => x.IsDateBased).IsRequired();
        entity.Property(x => x.CalculationKind).HasConversion<int>().IsRequired();
        entity.Property(x => x.MinMajorUnits).IsRequired();
        entity.Property(x => x.MinMinorUnits).IsRequired();
        entity.Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();

        entity.HasQueryFilter(x => !x.IsDeleted);
    }
}
