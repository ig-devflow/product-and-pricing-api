using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Persistence.Configuration.ReferenceData;

internal sealed class ContentTemplateConfiguration : IEntityTypeConfiguration<ContentTemplate>
{
    public void Configure(EntityTypeBuilder<ContentTemplate> entity)
    {
        entity.ToTable("ContentTemplate", "ReferenceData");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(x => x.UsageDescription)
            .HasMaxLength(500)
            .IsRequired(false);

        entity.Property(x => x.Scope)
            .HasConversion<short>()
            .HasColumnType("smallint")
            .IsRequired();

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}