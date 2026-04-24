namespace ProductsAndPricingNew.Domain.Common.Errors;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}