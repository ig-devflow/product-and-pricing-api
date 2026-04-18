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
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        b.Property(x => x.ShowInDropdown).IsRequired();
        b.Property(x => x.IsActive).IsRequired();

        b.Property(x => x.TermsAndConditions);
        b.Property(x => x.GroupsPaymentTerms);

        b.Property(x => x.WebsiteUrl)
            .HasMaxLength(500);

        b.Property(x => x.HeadOfficeEmail)
            .HasMaxLength(320);

        b.Property(x => x.HeadOfficeTelephoneNo)
            .HasMaxLength(50);

        b.OwnsOne(x => x.ContactAddress, owned =>
        {
            owned.Property(x => x.Line1)
                .HasColumnName("AddressLine1")
                .HasMaxLength(200);

            owned.Property(x => x.Line2)
                .HasColumnName("AddressLine2")
                .HasMaxLength(200);

            owned.Property(x => x.Line3)
                .HasColumnName("AddressLine3")
                .HasMaxLength(200);

            owned.Property(x => x.Line4)
                .HasColumnName("AddressLine4")
                .HasMaxLength(200);

            owned.Property(x => x.CountryId)
                .HasColumnName("AddressCountryId");
        });

        b.Navigation(x => x.ContactAddress).IsRequired(false);

        b.Ignore(x => x.DomainEvents);
    }
}