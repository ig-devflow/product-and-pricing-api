using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

public sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> b)
    {
        b.ToTable("Course");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.DivisionId).IsRequired();
        b.Property(x => x.IsActive).IsRequired();
        b.Property(x => x.CourseLanguageId).IsRequired();
        b.Property(x => x.CourseIntensityId).IsRequired();
        b.Property(x => x.UnitTypeId).IsRequired();
        b.Property(x => x.MinimumAge);
        b.Property(x => x.MinimumWeeks);
        b.Property(x => x.AccountCategoryId);
        b.Property(x => x.ProductCategoryId);
        b.Property(x => x.OfferingsClosureDate);

        b.ComplexProperty(x => x.FinanceCodes, complex =>
        {
            complex.Property(p => p.GeneralLedgerCode)
                .HasColumnName("GeneralLedgerCode")
                .HasMaxLength(10);

            complex.Property(p => p.CostCentreCode)
                .HasColumnName("CostCentreCode")
                .HasMaxLength(10);
        });

        b.HasIndex(x => new { x.DivisionId, x.Name });

        b.ConfigureAuditMetadata(x => x.AuditMetadata);
        b.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        b.Property(x => x.RowVersion).IsRowVersion();

        b.Ignore(x => x.DomainEvents);
    }
}