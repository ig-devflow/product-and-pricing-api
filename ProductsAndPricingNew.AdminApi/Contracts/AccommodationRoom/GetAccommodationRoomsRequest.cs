namespace ProductsAndPricingNew.AdminApi.Contracts.AccommodationRoom;

public sealed record GetAccommodationRoomsRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null
);