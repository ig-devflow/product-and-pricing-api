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

    public DbSet<Division> Divisions => Set<Division>();
    public DbSet<AccountCategory> AccountCategories => Set<AccountCategory>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<AddOn> AddOns => Set<AddOn>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<TransferPort> TransferPorts => Set<TransferPort>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsAndPricingDbContext).Assembly);
    }
}