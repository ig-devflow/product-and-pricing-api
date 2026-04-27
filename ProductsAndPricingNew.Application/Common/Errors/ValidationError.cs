using FluentResults;

namespace ProductsAndPricingNew.Application.Common.Errors;

public sealed class ValidationError : Error
{
    public const string Code = "validation_error";

    public ValidationError(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;

        Metadata.Add("Code", Code);
        Metadata.Add("Errors", errors);
    }

    public ValidationError(string message)
        : this(new Dictionary<string, string[]>
        {
            [string.Empty] = [message]
        })
    {
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}