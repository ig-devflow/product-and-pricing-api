using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Persistence.Interceptors;
using ProductsAndPricingNew.Persistence.Queries.Configuration;
using ProductsAndPricingNew.Persistence.Repositories;

namespace ProductsAndPricingNew.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        string connectionString = configuration.GetConnectionString("ProductsAndPricing")
                                  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<ProductsAndPricingDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);
            options.AddInterceptors(
                sp.GetRequiredService<AuditSaveChangesInterceptor>(),
                sp.GetRequiredService<SoftDeleteInterceptor>());
        });

        services.AddSingleton<ISqlConnectionFactory>(new SqlConnectionFactory(connectionString));

        services.Scan(scan => scan
            .FromAssemblyOf<DivisionRepository>()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")), publicOnly: false)
            .AsMatchingInterface()
            .WithScopedLifetime()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Queries")), publicOnly: false)
            .AsMatchingInterface()
            .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}