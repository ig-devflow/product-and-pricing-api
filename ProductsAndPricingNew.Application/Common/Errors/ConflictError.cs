using FluentResults;

namespace ProductsAndPricingNew.Application.Common.Errors;

public sealed class ConflictError : Error
{
    public ConflictError(string message) : base(message)
    {
    }
}