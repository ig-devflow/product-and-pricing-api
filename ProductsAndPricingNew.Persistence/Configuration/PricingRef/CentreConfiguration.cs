using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using CentreContactTypeEntity = ProductsAndPricingNew.Domain.ReferenceData.CentreContactType;

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
        entity.Property(x => x.IsPhysicalCentre).IsRequired();

        entity.Property(x => x.CurrencyId).IsRequired();

        entity.Property(x => x.PrintFormatId).IsRequired();

        entity.HasOne<PrintFormat>()
            .WithMany()
            .HasForeignKey(x => x.PrintFormatId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.Property(x => x.Code)
            .HasMaxLength(Centre.Rules.CodeMaxLength)
            .IsRequired();

        entity.Property(x => x.SchoolSponsorshipNumber)
            .HasMaxLength(Centre.Rules.LegalTextMaxLength);

        entity.Property(x => x.VatNumber)
            .HasMaxLength(Centre.Rules.LegalTextMaxLength);

        entity.Property(x => x.RegistrationNumber)
            .HasMaxLength(Centre.Rules.LegalTextMaxLength);

        entity.Property(x => x.VatExemptionNumber)
            .HasMaxLength(Centre.Rules.LegalTextMaxLength);

        entity.Property(x => x.ChequePayableTo)
            .HasMaxLength(Centre.Rules.LegalTextMaxLength);

        entity.Property(x => x.Guarantees)
            .HasConversion(Converters.Percentage)
            .HasColumnName("Guarantees")
            .HasPrecision(18, 2)
            .IsRequired(false);

        entity.Property(x => x.IndividualsRatio)
            .HasConversion(Converters.Percentage)
            .HasColumnName("IndividualsRatio")
            .HasPrecision(18, 2)
            .IsRequired(false);

        entity.Property(x => x.StaffingRatio)
            .HasConversion(Converters.Percentage)
            .HasColumnName("StaffingRatio")
            .HasPrecision(18, 2)
            .IsRequired(false);

        entity.Property(x => x.EmptyBeds)
            .HasConversion(Converters.Percentage)
            .HasColumnName("EmptyBeds")
            .HasPrecision(18, 2)
            .IsRequired(false);

        entity.ConfigureAddress(x => x.ContactAddress, "Contact");
        entity.ConfigureBanner(x => x.LogoImage, "Logo");
        entity.ConfigureAuditAndConcurrency();

        entity.Property(x => x.GeneralEmail)
            .HasConversion(Converters.EmailAddress)
            .HasColumnName("GeneralEmail")
            .HasMaxLength(EmailAddress.Rules.MaxLength)
            .IsRequired(false);

        entity.Property(x => x.AccommodationEmail)
            .HasConversion(Converters.EmailAddress)
            .HasColumnName("AccommodationEmail")
            .HasMaxLength(EmailAddress.Rules.MaxLength)
            .IsRequired(false);

        entity.Property(x => x.Telephone)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("Telephone")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength)
            .IsRequired(false);

        entity.Property(x => x.EmergencyTelephone)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("EmergencyTelephone")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength)
            .IsRequired(false);

        entity.Property(x => x.TransferEmergencyTelephone)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("TransferEmergencyTelephone")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength)
            .IsRequired(false);

        entity.Property(x => x.BrandColor)
            .HasConversion(Converters.HexColor)
            .HasColumnName("BrandColor")
            .HasMaxLength(HexColor.Rules.MaxLengthWithHash)
            .IsFixedLength()
            .IsRequired();

        entity.ComplexProperty(x => x.BankDetails, bank =>
        {
            bank.Property(x => x.BeneficiaryName)
                .HasColumnName("BankBeneficiaryName")
                .HasMaxLength(CentreBankDetails.Rules.MaxLength);

            bank.Property(x => x.AccountNumber)
                .HasColumnName("BankAccountNumber")
                .HasMaxLength(CentreBankDetails.Rules.MaxLength);

            bank.Property(x => x.BankName)
                .HasColumnName("BankName")
                .HasMaxLength(CentreBankDetails.Rules.MaxLength);

            bank.ComplexProperty(x => x.Identifiers, ids =>
            {
                ids.Property(x => x.Iban).HasColumnName("Iban").HasMaxLength(BankIdentifiers.Rules.MaxLength);
                ids.Property(x => x.SwiftCode).HasColumnName("SwiftCode").HasMaxLength(BankIdentifiers.Rules.MaxLength);
                ids.Property(x => x.BranchCode).HasColumnName("BranchCode").HasMaxLength(BankIdentifiers.Rules.MaxLength);
                ids.Property(x => x.AbaRoutingNo).HasColumnName("AbaRoutingNo").HasMaxLength(BankIdentifiers.Rules.MaxLength);
                ids.Property(x => x.AchAba).HasColumnName("AchAba").HasMaxLength(BankIdentifiers.Rules.MaxLength);
            });

            bank.ConfigureAddress(x => x.BankAddress, "Bank");
            bank.ConfigureAddress(x => x.BeneficiaryBankAddress, "Beneficiary");

            bank.ComplexProperty(x => x.Intermediary, inter =>
            {
                inter.Property(x => x.BankName)
                    .HasColumnName("IntermediaryBankName")
                    .HasMaxLength(IntermediaryBank.Rules.MaxLength);

                inter.Property(x => x.SwiftCode)
                    .HasColumnName("IntermediarySwiftCode")
                    .HasMaxLength(IntermediaryBank.Rules.MaxLength);

                inter.ConfigureAddress(x => x.BankAddress, "Intermediary");
            });
        });

        entity.OwnsMany(x => x.Contacts, contact =>
        {
            contact.ToTable("CentreContacts", "PricingRef");
            contact.WithOwner().HasForeignKey("CentreId");
            contact.HasKey("CentreId", nameof(CentreContact.ContactTypeId));

            contact.Property(x => x.ContactTypeId).IsRequired();

            contact.HasOne<CentreContactTypeEntity>()
                .WithMany()
                .HasForeignKey(x => x.ContactTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            contact.Property(x => x.Name).HasMaxLength(CentreContact.Rules.NameMaxLength).IsRequired();
            contact.Property(x => x.Email)
                .HasConversion(Converters.EmailAddress)
                .HasColumnName("Email")
                .HasMaxLength(EmailAddress.Rules.MaxLength)
                .IsRequired(false);

            contact.ConfigureBanner(x => x.SignatureImage, "Signature");
        });

        entity.HasIndex(x => x.Name).IsUnique();
        entity.HasIndex(x => x.Code).IsUnique();
    }
}