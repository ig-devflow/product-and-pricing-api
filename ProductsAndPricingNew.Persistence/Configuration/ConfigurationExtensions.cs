using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductsAndPricingNew.Domain.Entities.Products;
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

    public static void ConfigureAddress<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, Address>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, address =>
        {
            address.Property(x => x.Street)
                .HasColumnName("Street")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.District)
                .HasColumnName("District")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.City)
                .HasColumnName("City")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(Address.Rules.AddressFieldMaxLength);

            address.Property(x => x.CountryId)
                .HasColumnName("CountryId");
        });
    }

    // public static void ConfigureAddress<TEntity, TDependent>(
    //     this OwnedNavigationBuilder<TEntity, TDependent> builder,
    //     Expression<Func<TDependent, Address>> propertyExpression,
    //     string prefix)
    //     where TEntity : class
    //     where TDependent : class
    // {
    //     builder.ComplexProperty(propertyExpression, address =>
    //     {
    //         address.Property(x => x.CountryId)
    //             .HasColumnName($"{prefix}CountryId");
    //
    //         address.Property(x => x.Street)
    //             .HasColumnName($"{prefix}Street")
    //             .HasMaxLength(Address.Rules.AddressFieldMaxLength);
    //
    //         address.Property(x => x.District)
    //             .HasColumnName($"{prefix}District")
    //             .HasMaxLength(Address.Rules.AddressFieldMaxLength);
    //
    //         address.Property(x => x.City)
    //             .HasColumnName($"{prefix}City")
    //             .HasMaxLength(Address.Rules.AddressFieldMaxLength);
    //
    //         address.Property(x => x.PostalCode)
    //             .HasColumnName($"{prefix}PostalCode")
    //             .HasMaxLength(Address.Rules.AddressFieldMaxLength);
    //     });
    // }

    public static void ConfigureBanner<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, ImageFile>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, image =>
        {
            image.Property(x => x.Data)
                .HasColumnName("BannerData")
                .HasColumnType("varbinary(max)")
                .HasField("_data")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            image.Property(x => x.ContentType)
                .HasColumnName("BannerContentType")
                .HasMaxLength(ImageFile.Rules.ContentTypeMaxLength);

            image.Property(x => x.FileName)
                .HasColumnName("BannerFileName")
                .HasMaxLength(ImageFile.Rules.FileNameMaxLength);
        });
    }

    public static void ConfigureEmailAddress<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, EmailAddress>> propertyExpression, bool required = false)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, email =>
        {
            var property = email.Property(x => x.Value)
                .HasColumnName("Email")
                .HasMaxLength(EmailAddress.Rules.MaxLength);

            if (required)
                property.IsRequired();
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

    public static void ConfigureHexColor<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, HexColor>> propertyExpression, bool required = false)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, color =>
        {
            var property = color.Property(x => x.Value)
                .HasColumnName("HexColor")
                .HasMaxLength(HexColor.Rules.MaxLengthWithHash)
                .IsFixedLength();

            if (required)
                property.IsRequired();
        });
    }

    public static void ConfigureTelephoneNumber<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TelephoneNumber>> propertyExpression, bool required = false)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, telephone =>
        {
            var property = telephone.Property(x => x.Value)
                .HasColumnName("TelephoneNumber")
                .HasMaxLength(TelephoneNumber.Rules.MaxLength);

            if (required)
                property.IsRequired();
        });
    }

    public static void ConfigureWebsiteUrl<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, WebsiteUrl>> propertyExpression, bool required = false)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, url =>
        {
            var property = url.Property(x => x.Value)
                .HasColumnName("WebsiteUrl")
                .HasMaxLength(WebsiteUrl.Rules.MaxLength);

            if (required)
                property.IsRequired();
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

    private static readonly ValueConverter<WebsiteUrl, string?> WebsiteUrlConverter = new(
        url => url.IsEmpty ? null : url.Value,
        value => WebsiteUrl.Create(value).EnsureNotEmpty(nameof(WebsiteUrl)));

    private static readonly ValueConverter<EmailAddress?, string?> NullableEmailAddressConverter = new(
        email => email.HasValue && !email.Value.IsEmpty ? email.Value.Value : null,
        value => string.IsNullOrWhiteSpace(value) ? null : EmailAddress.Create(value));

    private static readonly ValueConverter<TelephoneNumber?, string?> NullableTelephoneNumberConverter = new(
        phone => phone.HasValue && !phone.Value.IsEmpty ? phone.Value.Value : null,
        value => string.IsNullOrWhiteSpace(value) ? null : TelephoneNumber.Create(value));

    private static readonly ValueConverter<HexColor?, string?> NullableHexColorConverter = new(
        color => color.HasValue && !color.Value.IsEmpty ? color.Value.Value : null,
        value => string.IsNullOrWhiteSpace(value) ? null : HexColor.Create(value));
}