using Microsoft.Extensions.DependencyInjection;
using ProductsAndPricingNew.Application.Behaviors;
using FluentValidation;

namespace ProductsAndPricingNew.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            //cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining<ValidationBehavior>();

        return services;
    }
}