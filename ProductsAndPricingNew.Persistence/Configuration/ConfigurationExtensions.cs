using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

    public static void ConfigureFinanceCodes<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, FinanceCodes>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, financeCodes =>
        {
            financeCodes.Property(p => p.GeneralLedgerCode)
                .HasColumnName("GeneralLedgerCode")
                .HasMaxLength(FinanceCodes.Rules.GeneralLedgerCodeMaxLength);

            financeCodes.Property(p => p.CostCentreCode)
                .HasColumnName("CostCentreCode")
                .HasMaxLength(FinanceCodes.Rules.CostCentreCodeMaxLength);
        });
    }
}