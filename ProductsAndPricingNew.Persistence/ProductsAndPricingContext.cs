using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.CancellationFees;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Persistence;

internal sealed class ProductsAndPricingDbContext : DbContext
{
    public ProductsAndPricingDbContext(DbContextOptions<ProductsAndPricingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ruleset> Rulesets => Set<Ruleset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsAndPricingDbContext).Assembly);
    }
}