namespace ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;

internal interface IAccommodationCommandPayload
{
    string Name { get; }
    int AccommodationTypeId { get; }
    bool IsActive { get; }
    int MinimumStayInWeeks { get; }
    int? AgeFrom { get; }
    int? AgeTo { get; }
    bool IsCommitted { get; }
    bool IsNonCommitted { get; }
}