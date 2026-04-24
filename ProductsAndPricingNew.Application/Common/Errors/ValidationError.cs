using FluentResults;

namespace ProductsAndPricingNew.Application.Common.Errors;

public class ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
    }
}