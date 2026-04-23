using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Persistence.Interceptors;
using ProductsAndPricingNew.Persistence.Queries.Configuration;
using ProductsAndPricingNew.Persistence.Queries.Division;
using ProductsAndPricingNew.Persistence.Repositories;

namespace ProductsAndPricingNew.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        string connectionString = configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<ProductsAndPricingDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);
            options.AddInterceptors(
                sp.GetRequiredService<AuditSaveChangesInterceptor>(),
                sp.GetRequiredService<SoftDeleteInterceptor>());
        });

        services.AddSingleton<ISqlConnectionFactory>(new SqlConnectionFactory(connectionString));

        services.AddScoped<IDivisionQueries, DivisionQueries>();

        services.AddScoped<IDivisionRepository, DivisionRepository>();
        services.AddScoped<IAccountCategoryRepository, AccountCategoryRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IAddOnRepository, AddOnRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<ITransferPortRepository, TransferPortRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}