using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

public sealed class AddOnConfiguration : IEntityTypeConfiguration<AddOn>
{
    public void Configure(EntityTypeBuilder<AddOn> b)
    {
        b.ToTable("AddOn");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.DivisionId).IsRequired();
        b.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        b.Property(x => x.IsActive).IsRequired();
        b.Property(x => x.AddOnTypeId).IsRequired();
        b.Property(x => x.UnitTypeId).IsRequired();

        b.Property(x => x.MinimumAge);
        b.Property(x => x.AccountCategoryId);
        b.Property(x => x.ProductCategoryId);

        b.Property(x => x.OneToOneLessonsPerWeek)
            .HasColumnName("OneToOneLessonsPerWeek");

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
        b.HasIndex(x => x.AccountCategoryId);
        b.HasIndex(x => x.ProductCategoryId);
        b.HasIndex(x => x.AddOnTypeId);
        b.HasIndex(x => x.UnitTypeId);

        b.Ignore(x => x.DomainEvents);
    }
}