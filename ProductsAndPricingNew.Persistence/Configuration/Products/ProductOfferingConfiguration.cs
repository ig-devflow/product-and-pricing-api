using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Offerings;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class ProductOfferingConfiguration : IEntityTypeConfiguration<ProductOffering>
{
    public void Configure(EntityTypeBuilder<ProductOffering> entity)
    {
        entity.ToTable("ProductOffering", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.SchoolId).IsRequired();
        entity.Property(x => x.UnitTypeId).IsRequired();
        entity.Property(x => x.UnitTypeIsDateBased).IsRequired();

        entity.ComplexProperty(x => x.Product, p =>
        {
            p.Property(v => v.Kind).HasColumnName("ProductKind").HasConversion<int>().IsRequired();
            p.Property(v => v.Id).HasColumnName("ProductDefinitionId").IsRequired();
        });

        entity.ComplexProperty(p => p.Closure, c =>
        {
            c.Property(x => x.DateStrategiesClosureDate).HasColumnName("DateStrategiesClosureDate");
            c.Property(x => x.DecommissionDate).HasColumnName("DecommissionDate");
        });

        entity.HasIndex(x => x.SchoolId);

        // TODO: enforce uniqueness of (SchoolId, ProductKind, ProductDefinitionId) at the DB level
        // via a migration — EF 10 cannot index members of a ComplexProperty through the fluent API.

        entity.OwnsMany(x => x.ScheduleSegments, segment =>
        {
            segment.ToTable("ScheduleSegment", "Product");
            segment.WithOwner().HasForeignKey("ProductOfferingId");
            segment.Property<int>("Id").ValueGeneratedOnAdd();
            segment.HasKey("Id");

            segment.Property(x => x.Kind).HasColumnName("ScheduleKind").HasConversion<int>().IsRequired();
            segment.Property(x => x.From).IsRequired();
            segment.Property(x => x.To);
            segment.Property(x => x.RecurrenceWeekdayMask).IsRequired();

            segment.Ignore(x => x.EffectiveEnd);
            segment.Ignore(x => x.IsOpenEnded);
            segment.Ignore(x => x.RecurrenceWeekdays);

            segment.OwnsMany(x => x.Dates, date =>
            {
                date.ToTable("ScheduleSegmentDate", "Product");
                date.WithOwner().HasForeignKey("ScheduleSegmentId");
                date.Property<int>("Id").ValueGeneratedOnAdd();
                date.HasKey("Id");

                date.Property(x => x.Start).IsRequired();
                date.Property(x => x.End);
            })
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        })
        .UsePropertyAccessMode(PropertyAccessMode.Field);

        entity.ConfigureAuditAndConcurrency();
    }
}