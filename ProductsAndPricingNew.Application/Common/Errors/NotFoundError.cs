using FluentResults;

namespace ProductsAndPricingNew.Application.Common.Errors;

public sealed class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
    }
}