using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> entity)
    {
        entity.ToTable("Transfer", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.DivisionId).IsRequired();
        entity.Property(x => x.UnitTypeId).IsRequired();
        entity.Property(x => x.Name).HasMaxLength(Transfer.Rules.NameMaxLength).IsRequired();
        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.TransferTypeId).IsRequired();
        entity.Property(x => x.TransferPortId).IsRequired();

        entity.ComplexProperty(x => x.TimeWindow, tw =>
        {
            tw.Property(p => p.From).HasColumnName("TimeFrom");
            tw.Property(p => p.To).HasColumnName("TimeTo");
        });

        entity.ConfigureProductCategories(x => x.Categories);
        entity.ConfigureFinanceCodes(x => x.FinanceCodes);

        entity.Property(x => x.ClosurePolicy)
            .HasConversion(Converters.OfferingsClosurePolicy)
            .HasColumnName("OfferingsClosureDate");

        entity.HasIndex(x => new { x.DivisionId, x.Name });
        entity.HasIndex(x => x.TransferTypeId);
        entity.HasIndex(x => x.TransferPortId);

        entity.HasOne<Division>()
            .WithMany()
            .HasForeignKey(x => x.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ConfigureAuditAndConcurrency();
    }
}
