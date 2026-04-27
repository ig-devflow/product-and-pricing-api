using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Persistence.Configuration;

internal static class ConfigurationExtensions
{
    public static void ConfigureAuditMetadata<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, AuditMetadata>> propertyExpression)
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

    public static void ConfigureAddress<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, Address>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, address =>
        {
            address.Property(x => x.Street)
                .HasColumnName("Street")
                .HasMaxLength(50);

            address.Property(x => x.District)
                .HasColumnName("District")
                .HasMaxLength(50);

            address.Property(x => x.City)
                .HasColumnName("City")
                .HasMaxLength(50);

            address.Property(x => x.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(50);

            address.Property(x => x.CountryId)
                .HasColumnName("CountryId");
        });
    }
    public static void ConfigureBanner<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, ImageFile>> propertyExpression)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, image =>
        {
            image.Property(x => x.Data)
                .HasColumnName("BannerData")
                .HasColumnType("varbinary(max)");

            image.Property(x => x.ContentType)
                .HasColumnName("BannerContentType")
                .HasMaxLength(100);

            image.Property(x => x.FileName)
                .HasColumnName("BannerFileName")
                .HasMaxLength(255);
        });
    }
}