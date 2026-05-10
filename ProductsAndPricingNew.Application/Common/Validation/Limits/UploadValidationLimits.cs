using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Common.Validation.Limits;

internal static class UploadValidationLimits
{
    public const int MaxAccreditationBannerBytes = Division.Rules.AccreditationBannerMaxBytes;
}
