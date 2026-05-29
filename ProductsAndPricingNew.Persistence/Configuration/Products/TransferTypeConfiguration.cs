using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class TransferTypeConfiguration : IEntityTypeConfiguration<TransferType>
{
    public void Configure(EntityTypeBuilder<TransferType> entity)
    {
        entity.ToTable("TransferType", "Product");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(TransferType.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        entity.HasData(
            new { Id = 1, Name = "Arrival", IsDeleted = false },
            new { Id = 2, Name = "Departure", IsDeleted = false }
        );
    }
}
