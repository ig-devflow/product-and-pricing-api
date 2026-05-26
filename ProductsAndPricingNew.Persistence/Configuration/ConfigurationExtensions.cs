using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Persistence.Configuration;

internal static class ConfigurationExtensions
{
    public static EntityTypeBuilder<T> ConfigureAuditAndConcurrency<T>(this EntityTypeBuilder<T> builder)
        where T : AggregateRoot<int>
    {
        builder.ComplexProperty<AuditMetadata>(nameof(AggregateRoot<int>.AuditMetadata), audit =>
        {
            audit.Property(x => x.CreatedById).HasColumnName("CreatedById").IsRequired();
            audit.Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            audit.Property(x => x.UpdatedById).HasColumnName("UpdatedById").IsRequired();
            audit.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();
        });

        builder.Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();
        builder.Property(x => x.Version).IsRowVersion();
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Ignore(e => e.DomainEvents);

        return builder;
    }

    public static void ConfigureAddress<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, Address>> propertyExpression, string prefix)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, address =>
        {
            address.Property(x => x.Street).HasColumnName($"{prefix}Street").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.District).HasColumnName($"{prefix}District").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.City).HasColumnName($"{prefix}City").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.PostalCode).HasColumnName($"{prefix}PostalCode").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.CountryId).HasColumnName($"{prefix}CountryId");
        });
    }

    public static void ConfigureAddress<TComplex>(this ComplexPropertyBuilder<TComplex> builder, Expression<Func<TComplex, Address>> propertyExpression,
        string prefix)
    {
        builder.ComplexProperty(propertyExpression, address =>
        {
            address.Property(x => x.Street).HasColumnName($"{prefix}Street").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.District).HasColumnName($"{prefix}District").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.City).HasColumnName($"{prefix}City").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.PostalCode).HasColumnName($"{prefix}PostalCode").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.CountryId).HasColumnName($"{prefix}CountryId");
        });
    }

    public static void ConfigureAddress<TEntity, TDependent>(this OwnedNavigationBuilder<TEntity, TDependent> builder, Expression<Func<TDependent, Address>> propertyExpression, string prefix)
        where TEntity : class
        where TDependent : class
    {
        builder.OwnsOne(propertyExpression, address =>
        {
            address.Property(x => x.CountryId).HasColumnName($"{prefix}CountryId");
            address.Property(x => x.Street).HasColumnName($"{prefix}Street").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.District).HasColumnName($"{prefix}District").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.City).HasColumnName($"{prefix}City").HasMaxLength(Address.Rules.AddressFieldMaxLength);
            address.Property(x => x.PostalCode).HasColumnName($"{prefix}PostalCode").HasMaxLength(Address.Rules.AddressFieldMaxLength);
        });
    }

    public static void ConfigureBanner<TEntity, TDependent>(this OwnedNavigationBuilder<TEntity, TDependent> builder, Expression<Func<TDependent, ImageFile>> propertyExpression, string prefix)
        where TEntity : class
        where TDependent : class
    {
        builder.OwnsOne(propertyExpression, image =>
        {
            image.Property(x => x.Data).HasColumnName($"{prefix}Data").HasColumnType("varbinary(max)").HasField("_data").UsePropertyAccessMode(PropertyAccessMode.Field);
            image.Property(x => x.ContentType).HasColumnName($"{prefix}ContentType").HasMaxLength(ImageFile.Rules.ContentTypeMaxLength);
            image.Property(x => x.FileName).HasColumnName($"{prefix}FileName").HasMaxLength(ImageFile.Rules.FileNameMaxLength);
        });
    }

    public static void ConfigureBanner<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, ImageFile>> propertyExpression, string prefix)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, image =>
        {
            image.Property(x => x.Data).HasColumnName($"{prefix}Data").HasColumnType("varbinary(max)").HasField("_data").UsePropertyAccessMode(PropertyAccessMode.Field);
            image.Property(x => x.ContentType).HasColumnName($"{prefix}ContentType").HasMaxLength(ImageFile.Rules.ContentTypeMaxLength);
            image.Property(x => x.FileName).HasColumnName($"{prefix}FileName").HasMaxLength(ImageFile.Rules.FileNameMaxLength);
        });
    }

    public static void ConfigureAgeRange<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, AgeRange>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, ageRange =>
        {
            ageRange.Property(x => x.Minimum).HasColumnName("MinimumAge").HasColumnType("smallint").IsRequired(false);
            ageRange.Property(x => x.Maximum).HasColumnName("MaximumAge").HasColumnType("smallint").IsRequired(false);
        });
    }

    public static void ConfigureFinanceCodes<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, FinanceCodes>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, fc =>
        {
            fc.Property(x => x.GeneralLedgerCode).HasColumnName("GeneralLedgerCode").HasMaxLength(FinanceCodes.Rules.MaxLength);
            fc.Property(x => x.CostCentreCode).HasColumnName("CostCentreCode").HasMaxLength(FinanceCodes.Rules.MaxLength);
        });
    }

    public static void ConfigureProductCategories<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, ProductCategories>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, c =>
        {
            c.Property(x => x.AccountCategoryId).HasColumnName("AccountCategoryId").IsRequired();
            c.Property(x => x.ProductCategoryId).HasColumnName("ProductCategoryId").IsRequired();
        });
    }
}