using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.ReferenceData;

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
            .HasMaxLength(ContentTemplate.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.Description)
            .HasMaxLength(ContentTemplate.Rules.DescriptionMaxLength)
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