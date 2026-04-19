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

        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
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
                .HasMaxLength(50);

            complex.Property(p => p.CostCentreCode)
                .HasColumnName("CostCentreCode")
                .HasMaxLength(50);
        });

        b.HasIndex(x => new { x.DivisionId, x.Name });

        b.ComplexProperty(x => x.EditInfo, audit =>
        {
            audit.Property(x => x.CreatedBy)
                .HasColumnName("CreatedBy")
                .IsRequired();

            audit.Property(x => x.CreateTimestamp)
                .HasColumnName("CreateTimestamp")
                .IsRequired();

            audit.Property(x => x.UpdatedBy)
                .HasColumnName("UpdatedBy")
                .IsRequired();

            audit.Property(x => x.UpdateTimestamp)
                .HasColumnName("UpdateTimestamp")
                .IsRequired();
        });
        
        b.Property(x => x.RowVersion).IsRowVersion();
        
        b.Ignore(x => x.DomainEvents);
    }
}