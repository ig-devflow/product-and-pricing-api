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
        entity.Property(x => x.IsPhysicalCentre).IsRequired();

        entity.Property(x => x.CurrencyId).IsRequired();

        entity.Property(x => x.PrintFormat)
            .HasConversion<short>()
            .HasColumnType("smallint")
            .IsRequired();

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

            bank.ComplexProperty(x => x.Identifiers, ids => // CreateCentreTable
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
            contact.HasKey("CentreId", nameof(CentreContact.ContactType));

            contact.Property(x => x.ContactType).HasColumnType("smallint").IsRequired();
            contact.Property(x => x.Name).HasMaxLength(CentreContact.Rules.NameMaxLength).IsRequired();
            contact.Property(x => x.Email)
                .HasConversion(Converters.EmailAddress)
                .HasColumnName("Email")
                .HasMaxLength(EmailAddress.Rules.MaxLength);

            contact.ConfigureBanner(x => x.SignatureImage, "Signature");
        });

        entity.Property(x => x.Version).IsRowVersion();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        entity.Ignore(x => x.DomainEvents);
    }
}