using ProductsAndPricingNew.AdminApi.Infrastructure;
using ProductsAndPricingNew.AdminApi.Middleware;
using ProductsAndPricingNew.Domain.Abstractions;

namespace ProductsAndPricingNew.AdminApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, HttpCurrentUser>();
        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}