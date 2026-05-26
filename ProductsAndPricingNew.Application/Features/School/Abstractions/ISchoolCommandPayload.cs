using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Application.Features.School.Abstractions;

internal interface ISchoolCommandPayload
{
    string Name { get; }
    string Code { get; }
    int MinimumStayInWeeks { get; }
    int? AgeFrom { get; }
    int? AgeTo { get; }
    string? Telephone { get; }
    string? EmergencyTelephone { get; }
    AddressDto? ContactAddress { get; }
    string? FinanceCode { get; }
    bool LmsAccess { get; }
    bool IsActive { get; }
    DateOnly? DecommissionDate { get; }
}