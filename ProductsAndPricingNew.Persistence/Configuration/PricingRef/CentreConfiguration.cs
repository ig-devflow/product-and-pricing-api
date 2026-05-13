using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class CentreConfiguration : IEntityTypeConfiguration<Centre>
{
    public void Configure(EntityTypeBuilder<Centre> builder)
    {
        builder.ToTable("Centre", "PricingRef");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(Centre.Rules.NameMaxLength)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(Centre.Rules.CodeMaxLength)
            .IsRequired();

        builder.ConfigureAddress(x => x.ContactAddress, "Contact");
        builder.ConfigureAuditMetadata(x => x.AuditMetadata);

        builder.ComplexProperty(x => x.GeneralEmail, email =>
        {
            email.Property(x => x.Value)
                .HasColumnName("GeneralEmail")
                .HasMaxLength(EmailAddress.Rules.MaxLength);
        });

        builder.ComplexProperty(x => x.AccommodationEmail, email =>
        {
            email.Property(x => x.Value)
                .HasColumnName("AccommodationEmail")
                .HasMaxLength(EmailAddress.Rules.MaxLength);
        });

        builder.ComplexProperty(x => x.Telephone, tel =>
        {
            tel.Property(x => x.Value)
                .HasColumnName("Telephone")
                .HasMaxLength(TelephoneNumber.Rules.MaxLength);
        });

        builder.ComplexProperty(x => x.EmergencyTelephone, tel =>
        {
            tel.Property(x => x.Value)
                .HasColumnName("EmergencyTelephone")
                .HasMaxLength(TelephoneNumber.Rules.MaxLength);
        });

        builder.ComplexProperty(x => x.TransferEmergencyTelephone, tel =>
        {
            tel.Property(x => x.Value)
                .HasColumnName("TransferEmergencyTelephone")
                .HasMaxLength(TelephoneNumber.Rules.MaxLength);
        });

        // HexColor value object
        builder.ComplexProperty(x => x.BrandColor, color =>
        {
            color.Property(x => x.Value)
                .HasColumnName("BrandColor")
                .HasMaxLength(HexColor.Rules.MaxLengthWithHash)
                .IsFixedLength();
        });

        // ImageFile (Logo) value object
        builder.ComplexProperty(x => x.LogoImage, image =>
        {
            image.Property(x => x.Data)
                .HasColumnName("LogoData")
                .HasColumnType("varbinary(max)")
                .HasField("_data")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            image.Property(x => x.ContentType)
                .HasColumnName("LogoContentType")
                .HasMaxLength(ImageFile.Rules.ContentTypeMaxLength);

            image.Property(x => x.FileName)
                .HasColumnName("LogoFileName")
                .HasMaxLength(ImageFile.Rules.FileNameMaxLength);
        });

        builder.OwnsOne(x => x.BankDetails, bank =>
        {
            bank.ToTable("CentreBankDetails", "PricingRef");

            bank.WithOwner()
                .HasForeignKey("CentreId");

            bank.HasKey("CentreId");

            bank.Property(x => x.BeneficiaryName)
                .HasMaxLength(200);

            bank.Property(x => x.AccountNumber)
                .HasMaxLength(100);

            bank.Property(x => x.BankName)
                .HasMaxLength(200);

            bank.Property(x => x.Iban)
                .HasMaxLength(100);

            bank.Property(x => x.SwiftCode)
                .HasMaxLength(50);

            bank.Property(x => x.BranchCode)
                .HasMaxLength(50);

            bank.Property(x => x.AbaRoutingNo)
                .HasMaxLength(50);

            bank.Property(x => x.AchAba)
                .HasMaxLength(50);

            bank.Property(x => x.IntermediaryBankName)
                .HasMaxLength(200);

            bank.Property(x => x.IntermediarySwiftCode)
                .HasMaxLength(50);

            bank.ConfigureAddress(x => x.BankAddress, "Bank");
            bank.ConfigureAddress(x => x.BeneficiaryBankAddress, "Beneficiary");
            bank.ConfigureAddress(x => x.IntermediaryBankAddress, "Intermediary");
        });
    }
}