using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Persistence.Interceptors;
using ProductsAndPricingNew.Persistence.Options;
using ProductsAndPricingNew.Persistence.Queries.Configuration;
using ProductsAndPricingNew.Persistence.Repositories;

namespace ProductsAndPricingNew.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<ConnectionStringsOptions>()
            .Bind(configuration.GetSection(ConnectionStringsOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.ProductsAndPricing), "Connection string 'ProductsAndPricing' was not found.")
            .ValidateOnStart();

        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<ProductsAndPricingDbContext>((serviceProvider, optionsBuilder) =>
        {
            ConnectionStringsOptions connectionStrings = serviceProvider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;

            optionsBuilder.UseSqlServer(connectionStrings.ProductsAndPricing);

            optionsBuilder.AddInterceptors(
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<SoftDeleteInterceptor>());
        });

        services.AddSingleton<ISqlConnectionFactory>(serviceProvider =>
        {
            ConnectionStringsOptions connectionStrings = serviceProvider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;
            return new SqlConnectionFactory(connectionStrings.ProductsAndPricing);
        });

        services.Scan(scan => scan
            .FromAssemblyOf<DivisionRepository>()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")), publicOnly: false)
                .AsMatchingInterface()
                .WithScopedLifetime()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Query")), publicOnly: false)
                .AsMatchingInterface()
                .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}