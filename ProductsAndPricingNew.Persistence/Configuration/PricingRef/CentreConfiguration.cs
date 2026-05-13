using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class CentreConfiguration : IEntityTypeConfiguration<Centre>
{
    public void Configure(EntityTypeBuilder<Centre> builder)
    {
        builder.ToTable("Centres", "PricingRef");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(Centre.Rules.NameMaxLength)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(Centre.Rules.CodeMaxLength)
            .IsRequired();

        //builder.ConfigureAddress(x => x.ContactAddress, "Contact");

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

            // bank.ConfigureAddress(x => x.BankAddress, "Bank");
            // bank.ConfigureAddress(x => x.BeneficiaryBankAddress, "Beneficiary");
            // bank.ConfigureAddress(x => x.IntermediaryBankAddress, "Intermediary");
        });
    }
}