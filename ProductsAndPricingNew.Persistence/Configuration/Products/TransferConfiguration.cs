using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

public sealed class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> b)
    {
        b.ToTable("Transfer");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.DivisionId).IsRequired();

        b.Property(x => x.UnitTypeId).IsRequired();

        b.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.IsActive).IsRequired();

        b.Property(x => x.TransferTypeId).IsRequired();
        b.Property(x => x.TransferPortId).IsRequired();

        b.Property(x => x.TimeFrom);
        b.Property(x => x.TimeTo);

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
        b.HasIndex(x => x.TransferTypeId);
        b.HasIndex(x => x.TransferPortId);
        b.HasIndex(x => x.AccountCategoryId);
        b.HasIndex(x => x.ProductCategoryId);
        b.HasIndex(x => x.UnitTypeId);

        b.ConfigureAuditMetadata(x => x.AuditMetadata);
        b.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        b.Property(x => x.RowVersion).IsRowVersion();

        b.Ignore(x => x.DomainEvents);
    }
}