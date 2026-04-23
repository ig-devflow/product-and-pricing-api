using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

public sealed class DivisionConfiguration : IEntityTypeConfiguration<Division>
{
    public void Configure(EntityTypeBuilder<Division> b)
    {
        b.ToTable("Division");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.IsActive).IsRequired();

        b.Property(x => x.TermsAndConditions);
        b.Property(x => x.GroupsPaymentTerms);

        b.Property(x => x.WebsiteUrl)
            .HasMaxLength(50);

        b.Property(x => x.HeadOfficeEmail)
            .HasMaxLength(50);

        b.Property(x => x.HeadOfficeTelephoneNo)
            .HasMaxLength(50);

        b.ComplexProperty(x => x.ContactAddress, owned =>
        {
            owned.Property(x => x.Line1)
                .HasColumnName("AddressLine1")
                .HasMaxLength(50);

            owned.Property(x => x.Line2)
                .HasColumnName("AddressLine2")
                .HasMaxLength(50);

            owned.Property(x => x.Line3)
                .HasColumnName("AddressLine3")
                .HasMaxLength(50);

            owned.Property(x => x.Line4)
                .HasColumnName("AddressLine4")
                .HasMaxLength(50);

            owned.Property(x => x.CountryId)
                .HasColumnName("AddressCountryId");
        });

        b.ConfigureAuditMetadata(x => x.AuditMetadata);

        b.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => x.Name).IsUnique();

        b.Ignore(x => x.DomainEvents);
    }
}