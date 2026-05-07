using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.AdminApi.Errors;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Errors;

namespace ProductsAndPricingNew.AdminApi.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private const string ServerErrorCode = "server_error";

    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        ProblemDetails problem = exception switch
        {
            DomainException domainException => ApiProblemDetails.Create(
                httpContext,
                StatusCodes.Status400BadRequest,
                "Domain rule violation",
                domainException.Message,
                DomainRuleViolationError.Code),

            DbUpdateConcurrencyException => ApiProblemDetails.Create(
                httpContext,
                StatusCodes.Status409Conflict,
                "Request conflicts with current state",
                "The resource was modified by another user. Reload it and try again.",
                ConflictError.Code),

            _ => ApiProblemDetails.Create(
                httpContext,
                StatusCodes.Status500InternalServerError,
                "Server error",
                "An unexpected error occurred.",
                ServerErrorCode)
        };

        if (problem.Status >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception");
        }
        else
        {
            _logger.LogWarning(exception, "Handled exception");
        }

        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;

        bool written = await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });

        if (!written)
        {
            httpContext.Response.ContentType = ApiProblemDetails.ProblemJsonContentType;
            await httpContext.Response.WriteAsJsonAsync(problem, ct);
        }

        return true;
    }
}