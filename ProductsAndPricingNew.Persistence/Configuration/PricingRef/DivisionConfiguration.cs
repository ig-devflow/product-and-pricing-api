using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class DivisionConfiguration : IEntityTypeConfiguration<Division>
{
    public void Configure(EntityTypeBuilder<Division> entity)
    {
        entity.ToTable("Division", "PricingRef");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(x => x.IsActive).IsRequired();

        entity.Property(x => x.TermsAndConditions);
        entity.Property(x => x.GroupsPaymentTerms);

        entity.Property(x => x.WebsiteUrl)
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(x => x.HeadOfficeEmail)
            .HasMaxLength(50);

        entity.Property(x => x.HeadOfficeTelephoneNo)
            .HasMaxLength(50);

        entity.ConfigureAddress(x => x.ContactAddress);
        entity.ConfigureBanner(x => x.AccreditationBanner);
        entity.ConfigureAuditMetadata(x => x.AuditMetadata);

        entity.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.Version).IsRowVersion();

        entity.HasIndex(x => x.Name).IsUnique();

        entity.Ignore(x => x.DomainEvents);
    }
}