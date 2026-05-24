using Microsoft.Extensions.DependencyInjection;
using ProductsAndPricingNew.Application.Behaviors;
using ProductsAndPricingNew.Application.Features.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;
using FluentValidation;

namespace ProductsAndPricingNew.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly, includeInternalTypes: true);

        services.AddSingleton<IFieldCatalog, FieldCatalog>();
        services.AddSingleton<RuleSpecificationFactory>();

        return services;
    }
}