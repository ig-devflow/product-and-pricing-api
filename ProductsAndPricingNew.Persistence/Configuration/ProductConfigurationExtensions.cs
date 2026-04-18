using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration;

internal static class ProductConfigurationExtensions
{
    public static void ConfigureFinanceCodes<T>(
        this EntityTypeBuilder<T> b,
        string ledgerColumnName = "GeneralLedgerCode",
        string costCentreColumnName = "CostCentreCode")
        where T : class
    {
        b.ComplexProperty("FinanceCodes", complex =>
        {
            complex.Property<string?>(nameof(FinanceCodes.GeneralLedgerCode))
                .HasColumnName(ledgerColumnName)
                .HasMaxLength(50);

            complex.Property<string?>(nameof(FinanceCodes.CostCentreCode))
                .HasColumnName(costCentreColumnName)
                .HasMaxLength(50);
        });
    }
}