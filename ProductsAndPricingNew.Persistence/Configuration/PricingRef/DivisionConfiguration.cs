using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class DivisionConfiguration : IEntityTypeConfiguration<Division>
{
    public void Configure(EntityTypeBuilder<Division> entity)
    {
        entity.ToTable("Division", "PricingRef");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(Division.Rules.NameMaxLength)
            .IsRequired();

        entity.Property(x => x.IsActive).IsRequired();

        entity.Property(x => x.TermsAndConditions)
            .HasMaxLength(Division.Rules.TermsAndConditionsMaxLength);

        entity.Property(x => x.GroupsPaymentTerms)
            .HasMaxLength(Division.Rules.GroupsPaymentTermsMaxLength);

        entity.Property(x => x.WebsiteUrl)
            .HasConversion(Converters.WebsiteUrl)
            .HasColumnName("WebsiteUrl")
            .HasMaxLength(WebsiteUrl.Rules.MaxLength)
            .IsRequired();

        entity.Property(x => x.HeadOfficeEmail)
            .HasConversion(Converters.EmailAddress)
            .HasColumnName("HeadOfficeEmail")
            .HasMaxLength(EmailAddress.Rules.MaxLength);

        entity.Property(x => x.HeadOfficeTelephoneNo)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("HeadOfficeTelephoneNo")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength);

        entity.ConfigureAddress(x => x.ContactAddress, "Contact");
        entity.ConfigureBanner(x => x.AccreditationBanner, "Banner");
        entity.ConfigureAuditMetadata(x => x.AuditMetadata);

        entity.HasMany(x => x.Texts)
            .WithOne()
            .HasForeignKey(x => x.DivisionId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.Navigation(x => x.Texts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.Version).IsRowVersion();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        entity.Ignore(x => x.DomainEvents);
    }
}
