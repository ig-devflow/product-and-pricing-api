using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductsAndPricingNew.Persistence.Options;

namespace ProductsAndPricingNew.Persistence;

internal sealed class ProductsAndPricingDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ProductsAndPricingDbContext>
{
    public ProductsAndPricingDbContext CreateDbContext(string[] args)
    {
        string environment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? "Development";

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(GetConfigurationBasePath())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        string connectionString = configuration
            .GetSection(ConnectionStringsOptions.SectionName)
            .GetValue<string>(nameof(ConnectionStringsOptions.ProductsAndPricing))
            ?? throw new InvalidOperationException("Connection string 'ProductsAndPricing' was not found.");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'ProductsAndPricing' was empty.");

        var optionsBuilder = new DbContextOptionsBuilder<ProductsAndPricingDbContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new ProductsAndPricingDbContext(optionsBuilder.Options);
    }

    private static string GetConfigurationBasePath()
    {
        string current = Directory.GetCurrentDirectory();

        string[] candidates =
        [
            current,
            Path.Combine(current, "ProductsAndPricingNew.AdminApi"),
            Path.GetFullPath(Path.Combine(current, "..", "ProductsAndPricingNew.AdminApi"))
        ];

        return candidates.FirstOrDefault(path => File.Exists(Path.Combine(path, "appsettings.json")))
            ?? current;
    }
}