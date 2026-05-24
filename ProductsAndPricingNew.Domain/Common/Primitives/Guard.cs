using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.Common.Primitives;

internal static class Guard
{
    public static int PositiveId(int value, string fieldName)
    {
        if (value <= 0)
            throw new DomainException($"{fieldName} must be greater than zero.");

        return value;
    }
}