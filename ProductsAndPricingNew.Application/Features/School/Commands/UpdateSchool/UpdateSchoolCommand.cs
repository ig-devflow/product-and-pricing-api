using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.School.Abstractions;

namespace ProductsAndPricingNew.Application.Features.School.Commands.UpdateSchool;

public sealed record UpdateSchoolCommand(
    int Id,
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
    DateOnly? DecommissionDate,
    string Version
) : ICommand<Result<Unit>>, ISchoolCommandPayload;