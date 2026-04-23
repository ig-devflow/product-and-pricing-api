using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Persistence;

public sealed class ProductsAndPricingDbContext : DbContext
{
    public ProductsAndPricingDbContext(DbContextOptions<ProductsAndPricingDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsAndPricingDbContext).Assembly);
    }
}