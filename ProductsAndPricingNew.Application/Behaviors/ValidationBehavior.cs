using System.Reflection;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Errors;

namespace ProductsAndPricingNew.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ResultBase
{
    private readonly IReadOnlyCollection<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators.ToArray();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (_validators.Count > 0)
        {
            ValidationContext<TRequest> context = new(request);

            ValidationResult[] validationResults = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context, ct)));

            List<ValidationFailure> failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(failure => failure is not null)
                .ToList();

            if (failures.Count > 0)
            {
                ValidationError error = new(BuildValidationErrors(failures));
                return ResultResponseFactory.Fail<TResponse>(error);
            }
        }

        try
        {
            return await next(ct);
        }
        catch (DomainException exception)
        {
            return ResultResponseFactory.Fail<TResponse>(new ValidationError(exception.Message));
        }
    }

    private static IReadOnlyDictionary<string, string[]> BuildValidationErrors(IEnumerable<ValidationFailure> failures)
    {
        return failures
            .GroupBy(failure =>
                string.IsNullOrWhiteSpace(failure.PropertyName)
                    ? string.Empty
                    : failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(failure => failure.ErrorMessage)
                    .Distinct()
                    .ToArray());
    }

    private static class ResultResponseFactory
    {
        public static TResponse Fail<TResponseResult>(IError error)
            where TResponseResult : ResultBase
        {
            Type responseType = typeof(TResponseResult);

            if (responseType == typeof(Result))
                return (TResponse)(object)Result.Fail(error);

            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                Type valueType = responseType.GetGenericArguments()[0];

                MethodInfo failMethod = typeof(Result)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(method =>
                        method.Name == nameof(Result.Fail) &&
                        method.IsGenericMethodDefinition)
                    .Single(method =>
                    {
                        ParameterInfo[] parameters = method.GetParameters();

                        return parameters.Length == 1 &&
                               parameters[0].ParameterType.IsAssignableFrom(error.GetType());
                    });

                object? result = failMethod
                    .MakeGenericMethod(valueType)
                    .Invoke(null, new object[] { error });

                return (TResponse)(result
                    ?? throw new InvalidOperationException("Could not create failed Result response."));
            }

            throw new InvalidOperationException(
                $"Unsupported MediatR response type '{responseType.Name}'. " +
                "ValidationBehavior supports only Result and Result<T> responses.");
        }
    }
}