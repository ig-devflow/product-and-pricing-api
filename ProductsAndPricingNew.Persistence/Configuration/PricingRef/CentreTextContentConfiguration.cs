using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class CentreTextContentConfiguration : IEntityTypeConfiguration<CentreTextContent>
{
    public void Configure(EntityTypeBuilder<CentreTextContent> builder)
    {
        builder.ToTable("CentreTextContents", "PricingRef");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CentreId)
            .IsRequired();

        builder.Property(x => x.ContentTemplateId)
            .IsRequired();

        builder.Property(x => x.AudienceId)
            .IsRequired(false);

        builder.ComplexProperty(x => x.Text, text =>
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

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne<ContentTemplate>()
            .WithMany()
            .HasForeignKey(x => x.ContentTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Audience>()
            .WithMany()
            .HasForeignKey(x => x.AudienceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.CentreId,
            x.ContentTemplateId,
            x.AudienceId
        }).IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}