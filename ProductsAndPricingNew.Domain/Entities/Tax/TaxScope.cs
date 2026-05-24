using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.Entities.Tax;

/// <summary>
/// What a <see cref="TaxRegimen"/> applies to: either a whole country or a single centre.
/// Replaces the legacy pair of mutually-exclusive nullable Country/Centre columns — the "both"
/// and "neither" states are structurally unrepresentable. The positional constructor exists for
/// EF materialisation; domain code builds instances through the validating factory methods.
/// </summary>
public readonly record struct TaxScope(TaxScopeKind Kind, int TargetId)
{
    public static TaxScope ForCountry(int countryId) => Build(TaxScopeKind.Country, countryId);

    public static TaxScope ForCentre(int centreId) => Build(TaxScopeKind.Centre, centreId);

    private static TaxScope Build(TaxScopeKind kind, int targetId)
    {
        if (targetId <= 0)
            throw new DomainException($"Tax scope {kind} target id must be greater than zero.");

        return new TaxScope(kind, targetId);
    }
}
