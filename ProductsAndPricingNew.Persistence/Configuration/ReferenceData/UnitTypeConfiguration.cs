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
        entity.Property(x => x.Id).ValueGeneratedNever();

        entity.Property(x => x.Name).HasMaxLength(UnitType.Rules.NameMaxLength).IsRequired();
        entity.Property(x => x.IsDateBased).IsRequired();
        entity.Property(x => x.CalculationKind).HasConversion<int>().IsRequired();
        entity.Property(x => x.MinMajorUnits).IsRequired();
        entity.Property(x => x.MinMinorUnits).IsRequired();
        entity.Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();

        entity.HasQueryFilter(x => !x.IsDeleted);

        entity.HasData(
            new { Id = 0, Name = "Any", IsDateBased = false, CalculationKind = UnitCalculationKind.Any, MinMajorUnits = 0, MinMinorUnits = 0, IsDeleted = false },
            new { Id = 1, Name = "Fixed price", IsDateBased = false, CalculationKind = UnitCalculationKind.FixedPrice, MinMajorUnits = 1, MinMinorUnits = 0, IsDeleted = false },
            new { Id = 2, Name = "Days", IsDateBased = true, CalculationKind = UnitCalculationKind.Day, MinMajorUnits = 1, MinMinorUnits = 0, IsDeleted = false },
            new { Id = 3, Name = "Calendar Weeks", IsDateBased = true, CalculationKind = UnitCalculationKind.CalendarWeek, MinMajorUnits = 0, MinMinorUnits = 1, IsDeleted = false },
            new { Id = 4, Name = "Calendar Night Weeks", IsDateBased = true, CalculationKind = UnitCalculationKind.CalendarNightWeek, MinMajorUnits = 0, MinMinorUnits = 1, IsDeleted = false },
            new { Id = 5, Name = "Working Weeks", IsDateBased = true, CalculationKind = UnitCalculationKind.WorkingWeek, MinMajorUnits = 0, MinMinorUnits = 1, IsDeleted = false },
            new { Id = 6, Name = "Months", IsDateBased = true, CalculationKind = UnitCalculationKind.Month, MinMajorUnits = 1, MinMinorUnits = 0, IsDeleted = false });
    }
}
