using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class CentreContactConfiguration : IEntityTypeConfiguration<CentreContact>
{
    public void Configure(EntityTypeBuilder<CentreContact> builder)
    {
        builder.ToTable("CentreContacts", "PricingRef");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(CentreContact.Rules.NameMaxLength)
            .IsRequired();

        builder.Property(x => x.ContactType)
            .HasColumnType("smallint")
            .IsRequired();

        builder.ConfigureEmailAddress(x => x.Email, "Email");

        builder.ComplexProperty(x => x.SignatureImage, image =>
        {
            image.Property(x => x.Data)
                .HasColumnName("SignatureData")
                .HasColumnType("varbinary(max)")
                .HasField("_data")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            image.Property(x => x.ContentType)
                .HasColumnName("SignatureContentType")
                .HasMaxLength(ImageFile.Rules.ContentTypeMaxLength);

            image.Property(x => x.FileName)
                .HasColumnName("SignatureFileName")
                .HasMaxLength(ImageFile.Rules.FileNameMaxLength);
        });
    }
}