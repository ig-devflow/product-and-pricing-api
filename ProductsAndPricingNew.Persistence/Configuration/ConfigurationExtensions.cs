using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Persistence.Configuration;

public static class ConfigurationExtensions
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
}