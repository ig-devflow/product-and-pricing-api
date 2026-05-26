namespace ProductsAndPricingNew.AdminApi.Contracts.Accommodation;

public record UpdateAccommodationRequest(
    string Name,
    int AccommodationTypeId,
    bool IsActive,
    int MinimumStayInWeeks,
    int? AgeFrom,
    int? AgeTo,
    bool IsCommitted,
    bool IsNonCommitted,
    string Version
);