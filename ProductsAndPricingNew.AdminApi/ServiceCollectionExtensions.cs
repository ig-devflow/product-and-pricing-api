using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using ProductsAndPricingNew.AdminApi.Errors;
using ProductsAndPricingNew.AdminApi.Infrastructure;
using ProductsAndPricingNew.AdminApi.Middleware;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Domain.Abstractions;

namespace ProductsAndPricingNew.AdminApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    IReadOnlyDictionary<string, string[]> errors = ApiProblemDetails.FromModelState(context.ModelState);
                    ValidationProblemDetails problem = ApiProblemDetails.CreateValidation(context.HttpContext, errors);
                    BadRequestObjectResult result = new(problem);
                    result.ContentTypes.Add(ApiProblemDetails.ProblemJsonContentType);

                    return result;
                };
            });

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                ProblemDetails problem = context.ProblemDetails;
                problem.Instance ??= context.HttpContext.Request.Path;

                if (!problem.Extensions.ContainsKey("traceId"))
                    problem.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

                if (!problem.Extensions.ContainsKey("code"))
                    problem.Extensions["code"] = ApiProblemDetails.DefaultCodeForStatus(problem.Status);
            };
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Products and Pricing Admin API",
                Version = "v1",
                Description = "Admin endpoints for Products and Pricing."
            });

            string xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, HttpCurrentUser>();
        services.AddSingleton<ISystemClock, SystemClock>();

        services.AddAutoMapper(typeof(Program).Assembly);

        return services;
    }
}