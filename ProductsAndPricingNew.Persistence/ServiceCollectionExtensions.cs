using Microsoft.Extensions.DependencyInjection;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Persistence.Repositories;

namespace ProductsAndPricingNew.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IDivisionRepository, DivisionRepository>();
        services.AddScoped<IAccountCategoryRepository, AccountCategoryRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IAddOnRepository, AddOnRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<ITransferPortRepository, TransferPortRepository>();

        return services;
    }
}