using FluentResults;

namespace ProductsAndPricingNew.Application.Common.Errors;

public sealed class ConflictError : Error
{
    public const string Code = "conflict";

    public ConflictError(string message) : base(message)
    {
        Metadata.Add("Code", Code);
    }
}