using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence.Configuration.Products;

internal sealed class AccommodationConfiguration : IEntityTypeConfiguration<Accommodation>
{
    public void Configure(EntityTypeBuilder<Accommodation> entity)
    {
        entity.ToTable("Accommodation", "Product");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(Accommodation.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsActive).IsRequired();
        entity.Property(x => x.AccommodationTypeId).IsRequired();
        entity.Property(x => x.MinimumStayInWeeks).IsRequired();
        entity.Property(x => x.IsCommitted).IsRequired();
        entity.Property(x => x.IsNonCommitted).IsRequired();

        entity.ConfigureAgeRange(x => x.AgeRange);

        entity.HasIndex(x => x.Name);

        entity.ConfigureAuditAndConcurrency();
    }
}