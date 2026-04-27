using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductsAndPricingNew.Application.Common.Errors;

namespace ProductsAndPricingNew.AdminApi.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(
        this Result result,
        ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.NoContent();

        return ToFailureActionResult(result, controller);
    }

    public static ActionResult ToActionResult<T>(
        this Result<T> result,
        ControllerBase controller,
        Func<T, ActionResult>? onSuccess = null)
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

    private static ActionResult ToFailureActionResult(
        ResultBase result,
        ControllerBase controller)
    {
        ValidationError? validationError = result.Errors
            .OfType<ValidationError>()
            .FirstOrDefault();

        if (validationError is not null)
            return ToValidationProblem(validationError, controller);

        ProblemDetails problem = CreateProblemDetails(result);

        if (result.Errors.Any(error => error is NotFoundError))
            return controller.NotFound(problem);

        if (result.Errors.Any(error => error is ConflictError))
            return controller.Conflict(problem);

        return controller.BadRequest(problem);
    }

    private static ActionResult ToValidationProblem(
        ValidationError error,
        ControllerBase controller)
    {
        ModelStateDictionary modelState = new();

        foreach ((string field, string[] messages) in error.Errors)
        {
            foreach (string message in messages)
                modelState.AddModelError(field, message);
        }

        return controller.ValidationProblem(modelState);
    }

    private static ProblemDetails CreateProblemDetails(ResultBase result)
    {
        int statusCode = GetStatusCode(result);

        ProblemDetails problem = new()
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = string.Join("; ", result.Errors.Select(error => error.Message))
        };

        problem.Extensions["errors"] = result.Errors
            .Select(error => new
            {
                code = GetErrorCode(error),
                message = error.Message
            })
            .ToArray();

        return problem;
    }

    private static int GetStatusCode(ResultBase result)
    {
        if (result.Errors.Any(error => error is NotFoundError))
            return StatusCodes.Status404NotFound;

        if (result.Errors.Any(error => error is ConflictError))
            return StatusCodes.Status409Conflict;

        if (result.Errors.Any(error => error is ValidationError))
            return StatusCodes.Status400BadRequest;

        return StatusCodes.Status400BadRequest;
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status404NotFound => "Resource was not found",
        StatusCodes.Status409Conflict => "Request conflicts with current state",
        StatusCodes.Status400BadRequest => "Request validation failed",
        _ => "Request failed"
    };

    private static string GetErrorCode(IError error)
    {
        if (error.Metadata.TryGetValue("Code", out object? code) &&
            code is string stringCode)
        {
            return stringCode;
        }

        return "request_failed";
    }
}