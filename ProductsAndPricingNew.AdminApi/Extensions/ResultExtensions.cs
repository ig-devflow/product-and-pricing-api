using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Errors;
using ProductsAndPricingNew.Application.Common.Errors;

namespace ProductsAndPricingNew.AdminApi.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.NoContent();

        return ToFailureActionResult(result, controller);
    }

    public static ActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller, Func<T, ActionResult>? onSuccess = null)
    {
        if (result.IsSuccess)
        {
            if (onSuccess is not null)
                return onSuccess(result.Value);

            if (typeof(T) == typeof(Unit))
                return controller.NoContent();

            return controller.Ok(result.Value);
        }

        return ToFailureActionResult(result, controller);
    }

    private static ActionResult ToFailureActionResult(ResultBase result, ControllerBase controller)
    {
        ValidationError? validationError = result.Errors
            .OfType<ValidationError>()
            .FirstOrDefault();

        if (validationError is not null)
        {
            ValidationProblemDetails validationProblem = ApiProblemDetails.CreateValidation(controller.HttpContext, validationError.Errors);
            return ToObjectResult(validationProblem);
        }

        (int statusCode, string title, string code) = ResolveProblem(result);

        ProblemDetails problem = ApiProblemDetails.Create(
            controller.HttpContext,
            statusCode,
            title,
            string.Join("; ", result.Errors.Select(error => error.Message)),
            code);

        return ToObjectResult(problem);
    }

    private static ObjectResult ToObjectResult(ProblemDetails problem)
    {
        ObjectResult result = new(problem)
        {
            StatusCode = problem.Status
        };

        result.ContentTypes.Add(ApiProblemDetails.ProblemJsonContentType);
        return result;
    }

    private static (int StatusCode, string Title, string Code) ResolveProblem(ResultBase result)
    {
        if (result.Errors.Any(error => error is NotFoundError))
            return (StatusCodes.Status404NotFound, "Resource was not found", NotFoundError.Code);

        if (result.Errors.Any(error => error is ConflictError))
            return (StatusCodes.Status409Conflict, "Request conflicts with current state", ConflictError.Code);

        if (result.Errors.Any(error => error is DomainRuleViolationError))
            return (StatusCodes.Status400BadRequest, "Domain rule violation", DomainRuleViolationError.Code);

        return (StatusCodes.Status400BadRequest, "Request failed", GetErrorCode(result.Errors.FirstOrDefault()));
    }

    private static string GetErrorCode(IError? error)
    {
        if (error?.Metadata.TryGetValue("Code", out object? code) == true &&
            code is string stringCode)
        {
            return stringCode;
        }

        return "request_failed";
    }
}