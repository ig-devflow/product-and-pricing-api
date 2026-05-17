using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.School.Abstractions;

namespace ProductsAndPricingNew.Application.Features.School.Commands.CreateSchool;

public sealed record CreateSchoolCommand(
    int CentreId,
    string Name,
    string LegacyCode,
    int MinimumStayInWeeks,
    int? AgeFrom,
    int? AgeTo,
    string? Telephone,
    string? EmergencyTelephone,
    AddressDto? ContactAddress,
    string? FinanceCode,
    bool LmsAccess,
    bool IsActive,
    DateOnly? DecommissionDate
) : ICommand<Result<int>>, ISchoolCommandPayload;