using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> entity)
    {
        entity.ToTable("ProductCategory", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.DivisionId).IsRequired();

        entity.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.IsDeleted).IsRequired();

        entity.HasIndex(x => new { x.DivisionId, x.Name });
        entity.HasIndex(x => new { x.DivisionId, x.IsDeleted, x.IsActive });

        entity.ConfigureAuditMetadata(x => x.AuditMetadata);
        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.Version).IsRowVersion();

        entity.Ignore(x => x.DomainEvents);
    }
}