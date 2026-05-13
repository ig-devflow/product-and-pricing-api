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

        entity.ConfigureWebsiteUrl(x => x.WebsiteUrl, required: true);
        entity.ConfigureEmailAddress(x => x.HeadOfficeEmail);
        entity.ConfigureTelephoneNumber(x => x.HeadOfficeTelephoneNo);
        entity.ConfigureAddress(x => x.ContactAddress, "Contact");
        entity.ConfigureBanner(x => x.AccreditationBanner);
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
