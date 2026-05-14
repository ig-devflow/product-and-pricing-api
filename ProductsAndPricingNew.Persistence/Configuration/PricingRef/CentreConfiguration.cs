using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class CentreConfiguration : IEntityTypeConfiguration<Centre>
{
    public void Configure(EntityTypeBuilder<Centre> entity)
    {
        entity.ToTable("Centre", "PricingRef");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(Centre.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsActive).IsRequired();

        entity.Property(x => x.Code)
            .HasMaxLength(Centre.Rules.CodeMaxLength)
            .IsRequired();

        entity.ConfigureAddress(x => x.ContactAddress, "Contact");
        entity.ConfigureBanner(x => x.LogoImage, "Logo");
        entity.ConfigureAuditMetadata(x => x.AuditMetadata);

        entity.Property(x => x.GeneralEmail)
            .HasConversion(Converters.EmailAddress)
            .HasColumnName("GeneralEmail")
            .HasMaxLength(EmailAddress.Rules.MaxLength);

        entity.Property(x => x.AccommodationEmail)
            .HasConversion(Converters.EmailAddress)
            .HasColumnName("AccommodationEmail")
            .HasMaxLength(EmailAddress.Rules.MaxLength);

        entity.Property(x => x.Telephone)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("Telephone")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength);

        entity.Property(x => x.EmergencyTelephone)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("EmergencyTelephone")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength);

        entity.Property(x => x.TransferEmergencyTelephone)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("TransferEmergencyTelephone")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength);

        entity.Property(x => x.BrandColor)
            .HasConversion(Converters.HexColor)
            .HasColumnName("BrandColor")
            .HasMaxLength(HexColor.Rules.MaxLengthWithHash)
            .IsFixedLength();

        entity.OwnsOne(x => x.BankDetails, bank =>
        {
            bank.ToTable("CentreBankDetails", "PricingRef");

            bank.WithOwner().HasForeignKey("CentreId");
            bank.HasKey("CentreId");

            bank.Property(x => x.BeneficiaryName).HasMaxLength(200);
            bank.Property(x => x.AccountNumber).HasMaxLength(100);
            bank.Property(x => x.BankName).HasMaxLength(200);
            bank.Property(x => x.Iban).HasMaxLength(100);
            bank.Property(x => x.SwiftCode).HasMaxLength(50);
            bank.Property(x => x.BranchCode).HasMaxLength(50);
            bank.Property(x => x.AbaRoutingNo).HasMaxLength(50);
            bank.Property(x => x.AchAba).HasMaxLength(50);
            bank.Property(x => x.IntermediaryBankName).HasMaxLength(200);
            bank.Property(x => x.IntermediarySwiftCode).HasMaxLength(50);

            bank.ConfigureAddress(x => x.BankAddress, "Bank");
            bank.ConfigureAddress(x => x.BeneficiaryBankAddress, "Beneficiary");
            bank.ConfigureAddress(x => x.IntermediaryBankAddress, "Intermediary");
        });

        entity.OwnsMany(x => x.Contacts, contact =>
        {
            contact.ToTable("CentreContacts", "PricingRef");

            contact.WithOwner().HasForeignKey("CentreId");

            contact.HasKey("CentreId", nameof(CentreContact.ContactType));

            contact.Property(x => x.ContactType)
                .HasColumnType("smallint")
                .IsRequired();

            contact.Property(x => x.Name)
                .HasMaxLength(CentreContact.Rules.NameMaxLength)
                .IsRequired();

            // contact.Property(x => x.IsDeleted)
            //     .IsRequired()
            //     .HasDefaultValue(false);

            contact.Property(x => x.Email)
                .HasConversion(Converters.EmailAddress)
                .HasColumnName("Email")
                .HasMaxLength(EmailAddress.Rules.MaxLength);

            contact.ConfigureBanner(x => x.SignatureImage, "Signature");
        });
    }
}