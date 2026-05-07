using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProductsAndPricingNew.AdminApi.Errors;

internal static class ApiProblemDetails
{
    public const string ProblemJsonContentType = "application/problem+json";

    public static ProblemDetails Create(
        HttpContext httpContext,
        int statusCode,
        string title,
        string detail,
        string code)
    {
        ProblemDetails problem = new()
        {
            Type = GetTypeUri(statusCode),
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        Enrich(problem, httpContext, code);

        return problem;
    }

    public static ValidationProblemDetails CreateValidation(HttpContext httpContext, IReadOnlyDictionary<string, string[]> errors)
    {
        ValidationProblemDetails problem = new(errors.ToDictionary(x => x.Key, x => x.Value))
        {
            Type = GetTypeUri(StatusCodes.Status400BadRequest),
            Title = "Validation failed",
            Status = StatusCodes.Status400BadRequest,
            Detail = "One or more validation errors occurred.",
            Instance = httpContext.Request.Path
        };

        Enrich(problem, httpContext, "validation_error");

        return problem;
    }

    public static IReadOnlyDictionary<string, string[]> FromModelState(ModelStateDictionary modelState)
    {
        return modelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!
                    .Errors.Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                            ? "The input was not valid."
                            : error.ErrorMessage)
                    .Distinct()
                    .ToArray());
    }

    public static string DefaultCodeForStatus(int? statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "bad_request",
        StatusCodes.Status401Unauthorized => "unauthorized",
        StatusCodes.Status403Forbidden => "forbidden",
        StatusCodes.Status404NotFound => "not_found",
        StatusCodes.Status405MethodNotAllowed => "method_not_allowed",
        StatusCodes.Status409Conflict => "conflict",
        StatusCodes.Status500InternalServerError => "server_error",
        _ => "request_failed"
    };

    private static void Enrich(ProblemDetails problem, HttpContext httpContext, string code)
    {
        problem.Extensions["code"] = code;
        problem.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
    }

    private static string GetTypeUri(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "https://httpstatuses.com/400",
        StatusCodes.Status401Unauthorized => "https://httpstatuses.com/401",
        StatusCodes.Status403Forbidden => "https://httpstatuses.com/403",
        StatusCodes.Status404NotFound => "https://httpstatuses.com/404",
        StatusCodes.Status405MethodNotAllowed => "https://httpstatuses.com/405",
        StatusCodes.Status409Conflict => "https://httpstatuses.com/409",
        StatusCodes.Status500InternalServerError => "https://httpstatuses.com/500",
        _ => "https://httpstatuses.com/400"
    };
}