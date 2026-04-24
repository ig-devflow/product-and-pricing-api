using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.Application.Common.Errors;

namespace ProductsAndPricingNew.AdminApi.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.NoContent();

        return CreateErrorResult(result);
    }

    public static ActionResult ToActionResult<T>(
        this Result<T> result,
        ControllerBase controller,
        Func<T, ActionResult>? onSuccess = null)
    {
        if (result.IsSuccess)
            return onSuccess is not null
                ? onSuccess(result.Value)
                : controller.Ok(result.Value);

        return CreateErrorResult(result);
    }

    private static ActionResult CreateErrorResult(ResultBase result)
    {
        int statusCode = GetStatusCode(result);

        ProblemDetails problem = new()
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = string.Join("; ", result.Errors.Select(static error => error.Message))
        };

        return new ObjectResult(problem)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCode(ResultBase result)
    {
        if (result.Errors.Any(static error => error is NotFoundError))
            return StatusCodes.Status404NotFound;

        if (result.Errors.Any(static error => error is ConflictError))
            return StatusCodes.Status409Conflict;

        return StatusCodes.Status400BadRequest;
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status404NotFound => "Resource was not found",
        StatusCodes.Status409Conflict => "Request conflicts with current state",
        _ => "Request failed"
    };
}