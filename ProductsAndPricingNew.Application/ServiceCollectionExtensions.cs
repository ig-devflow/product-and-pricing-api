using Microsoft.Extensions.DependencyInjection;
using ProductsAndPricingNew.Application.Behaviors;

namespace ProductsAndPricingNew.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });


        return services;
    }
}