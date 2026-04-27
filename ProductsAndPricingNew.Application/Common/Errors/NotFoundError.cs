using FluentResults;

namespace ProductsAndPricingNew.Application.Common.Errors;

public sealed class NotFoundError : Error
{
    public const string Code = "not_found";

    public NotFoundError(string message) : base(message)
    {
        Metadata.Add("Code", Code);
    }
}