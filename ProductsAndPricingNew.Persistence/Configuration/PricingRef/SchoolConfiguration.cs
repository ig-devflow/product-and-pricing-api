using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration.PricingRef;

internal sealed class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> entity)
    {
        entity.ToTable("School", "PricingRef");

        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasMaxLength(School.Rules.NameMaxLength)
            .IsRequired();
        
        entity.Property(x => x.LegacyCode)
            .HasMaxLength(School.Rules.LegacyCodeMaxLength)
            .IsRequired();

        entity.Property(x => x.MinimumStayInWeeks);

        // AgeRange
        
        entity.Property(x => x.Telephone)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("Telephone")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength);
        
        entity.Property(x => x.EmergencyTelephone)
            .HasConversion(Converters.TelephoneNumber)
            .HasColumnName("EmergencyTelephone")
            .HasMaxLength(TelephoneNumber.Rules.MaxLength);
        
        // FinanceCode
        
        entity.Property(x => x.LmsAccess)
            .IsRequired();
        
        entity.Property(x => x.IsActive)
            .IsRequired();
        
        entity.Property(x => x.DecommissionDate)
            .IsRequired(false);

        entity.ConfigureAddress(x => x.ContactAddress, "Contact");
        entity.ConfigureAuditMetadata(x => x.AuditMetadata);

        entity.Property(x => x.Version).IsRowVersion();

        entity.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        entity.Ignore(x => x.DomainEvents);
    }
}