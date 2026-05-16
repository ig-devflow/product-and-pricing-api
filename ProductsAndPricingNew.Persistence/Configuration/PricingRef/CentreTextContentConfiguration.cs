using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class CentreTextContentConfiguration : IEntityTypeConfiguration<CentreTextContent>
{
    public void Configure(EntityTypeBuilder<CentreTextContent> entity)
    {
        entity.ToTable("CentreTextContent", "PricingRef");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.CentreId)
            .IsRequired();

        entity.Property(x => x.ContentTemplateId)
            .IsRequired();

        entity.Property(x => x.AudienceId)
            .IsRequired(false);

        entity.ComplexProperty(x => x.Text, text =>
        {
            text.Property(x => x.Content)
                .HasColumnName("Content")
                .HasMaxLength(FormattedText.Rules.ContentMaxLength)
                .IsRequired();

            text.Property(x => x.Format)
                .HasColumnName("Format")
                .HasConversion<short>()
                .HasColumnType("smallint")
                .HasDefaultValue(ContentFormat.None)
                .IsRequired();
        });

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.HasOne<ContentTemplate>()
            .WithMany()
            .HasForeignKey(x => x.ContentTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<Audience>()
            .WithMany()
            .HasForeignKey(x => x.AudienceId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(x => new
        {
            x.CentreId,
            x.ContentTemplateId,
            x.AudienceId
        }).IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}