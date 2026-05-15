using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration;

internal static class ConfigurationExtensions
{
    public static void ConfigureAuditMetadata<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, AuditMetadata>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, audit =>
        {
            audit.Property(x => x.CreatedById)
                .HasColumnName("CreatedById")
                .IsRequired();

            audit.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            audit.Property(x => x.UpdatedById)
                .HasColumnName("UpdatedById")
                .IsRequired();

            audit.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .IsRequired();
        });
    }

    public static void ConfigureAddress<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, Address>> propertyExpression, string prefix)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, address =>
        {
            address.Property(x => x.Street)
                .HasColumnName($"{prefix}Street")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.District)
                .HasColumnName($"{prefix}District")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.City)
                .HasColumnName($"{prefix}City")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.PostalCode)
                .HasColumnName($"{prefix}PostalCode")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.CountryId)
                .HasColumnName($"{prefix}CountryId");
        });
    }

    public static void ConfigureAddress<TComplex>(
        this ComplexPropertyBuilder<TComplex> builder,
        Expression<Func<TComplex, Address>> propertyExpression,
        string prefix)
    {
        builder.ComplexProperty(propertyExpression, address =>
        {
            address.Property(x => x.Street)
                .HasColumnName($"{prefix}Street")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.District)
                .HasColumnName($"{prefix}District")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.City)
                .HasColumnName($"{prefix}City")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.PostalCode)
                .HasColumnName($"{prefix}PostalCode")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.CountryId)
                .HasColumnName($"{prefix}CountryId");
        });
    }

    public static void ConfigureAddress<TEntity, TDependent>(this OwnedNavigationBuilder<TEntity, TDependent> builder, Expression<Func<TDependent, Address>> propertyExpression, string prefix)
        where TEntity : class
        where TDependent : class
    {
        builder.OwnsOne(propertyExpression, address =>
        {
            address.Property(x => x.CountryId)
                .HasColumnName($"{prefix}CountryId");

            address.Property(x => x.Street)
                .HasColumnName($"{prefix}Street")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.District)
                .HasColumnName($"{prefix}District")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.City)
                .HasColumnName($"{prefix}City")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.PostalCode)
                .HasColumnName($"{prefix}PostalCode")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);
        });
    }

    public static void ConfigureBanner<TEntity, TDependent>(this OwnedNavigationBuilder<TEntity, TDependent> builder, Expression<Func<TDependent, ImageFile>> propertyExpression, string prefix)
        where TEntity : class
        where TDependent : class
    {
        builder.OwnsOne(propertyExpression, image =>
        {
            image.Property(x => x.Data)
                .HasColumnName($"{prefix}Data")
                .HasColumnType("varbinary(max)")
                .HasField("_data")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            image.Property(x => x.ContentType)
                .HasColumnName($"{prefix}ContentType")
                .HasMaxLength(ImageFile.Rules.ContentTypeMaxLength);

            image.Property(x => x.FileName)
                .HasColumnName($"{prefix}FileName")
                .HasMaxLength(ImageFile.Rules.FileNameMaxLength);
        });
    }

    public static void ConfigureBanner<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, ImageFile>> propertyExpression, string prefix)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, image =>
        {
            image.Property(x => x.Data)
                .HasColumnName($"{prefix}Data")
                .HasColumnType("varbinary(max)")
                .HasField("_data")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            image.Property(x => x.ContentType)
                .HasColumnName($"{prefix}ContentType")
                .HasMaxLength(ImageFile.Rules.ContentTypeMaxLength);

            image.Property(x => x.FileName)
                .HasColumnName($"{prefix}FileName")
                .HasMaxLength(ImageFile.Rules.FileNameMaxLength);
        });
    }

    public static void ConfigureAgeRange<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, AgeRange>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, ageRange =>
        {
            ageRange.Property(x => x.From)
                .HasColumnName("AgeFrom")
                .HasColumnType("smallint");

            ageRange.Property(x => x.To)
                .HasColumnName("AgeTo")
                .HasColumnType("smallint");
        });
    }

    // public static void ConfigureFinanceCodes<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, FinanceCodes>> propertyExpression)
    //     where TEntity : class
    // {
    //     builder.ComplexProperty(propertyExpression, financeCodes =>
    //     {
    //         financeCodes.Property(p => p.GeneralLedgerCode)
    //             .HasColumnName("GeneralLedgerCode")
    //             .HasMaxLength(FinanceCodes.Rules.GeneralLedgerCodeMaxLength);
    //
    //         financeCodes.Property(p => p.CostCentreCode)
    //             .HasColumnName("CostCentreCode")
    //             .HasMaxLength(FinanceCodes.Rules.CostCentreCodeMaxLength);
    //     });
    // }
}