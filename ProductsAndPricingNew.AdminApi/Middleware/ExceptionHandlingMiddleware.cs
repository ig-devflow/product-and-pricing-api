using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.Domain;
using ProductsAndPricingNew.Domain.Common.Errors;

namespace ProductsAndPricingNew.AdminApi.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        _logger.LogError(exception, "Unhandled exception");

        int statusCode = exception is DomainException
            ? StatusCodes.Status400BadRequest
            : StatusCodes.Status500InternalServerError;

        ProblemDetails problem = new()
        {
            Status = statusCode,
            Title = exception is DomainException
                ? "Domain rule violation"
                : "Server error",
            Detail = exception is DomainException
                ? exception.Message
                : "An unexpected error occurred."
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, ct);

        return true;
    }
}