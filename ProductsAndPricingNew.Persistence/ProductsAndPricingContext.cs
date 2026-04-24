using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence;

internal sealed class ProductsAndPricingDbContext : DbContext
{
    public ProductsAndPricingDbContext(DbContextOptions<ProductsAndPricingDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsAndPricingDbContext).Assembly);
    }
}