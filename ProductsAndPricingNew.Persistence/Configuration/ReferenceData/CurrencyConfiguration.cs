using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.ReferenceData;

internal sealed class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> entity)
    {
        entity.ToTable("Currency", "ReferenceData");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.IsoCode)
            .HasMaxLength(Currency.Rules.IsoCodeMaxLength)
            .IsRequired();

        entity.Property(x => x.Name)
            .HasMaxLength(Currency.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.Symbol)
            .HasConversion<string>()
            .HasMaxLength(Currency.Rules.SymbolMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.IsoCode)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}