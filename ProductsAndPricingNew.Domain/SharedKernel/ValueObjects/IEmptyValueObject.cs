using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public interface IEmptyValueObject
{
    bool IsEmpty { get; }
}

public static class EmptyValueObjectExtensions
{
    public static T EnsureNotEmpty<T>(this T value, string fieldName)
        where T : IEmptyValueObject
    {
        if (value.IsEmpty)
            throw new DomainException($"{fieldName} is required.");

        return value;
    }
}