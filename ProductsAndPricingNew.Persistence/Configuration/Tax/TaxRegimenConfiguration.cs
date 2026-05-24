using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Tax;

namespace ProductsAndPricingNew.Persistence.Configuration.Tax;

internal sealed class TaxRegimenConfiguration : IEntityTypeConfiguration<TaxRegimen>
{
    public void Configure(EntityTypeBuilder<TaxRegimen> b)
    {
        b.ToTable("TaxRegimen", "Tax");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.Name).HasMaxLength(TaxRegimen.Rules.NameMaxLength).IsRequired();
        b.Property(x => x.ValidFrom).IsRequired();
        b.Property(x => x.ValidTo);
        b.Property(x => x.IsActive).IsRequired();

        b.ComplexProperty(x => x.Scope, s =>
        {
            s.Property(v => v.Kind).HasConversion<int>().HasColumnName("ScopeKind").IsRequired();
            s.Property(v => v.TargetId).HasColumnName("ScopeTargetId").IsRequired();
        });

        // The relationship is mapped through the backing field; the read-only Bands
        // property must not be discovered as a second navigation over the same field.
        b.Ignore(x => x.Bands);

        b.HasMany<TaxBand>("_bands")
            .WithOne()
            .HasForeignKey(x => x.TaxRegimenId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Navigation("_bands")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        b.HasIndex(x => x.ValidFrom);

        b.ConfigureAuditAndConcurrency();
    }
}
