namespace ProductsAndPricingNew.Application.Features.Division.Models;

public sealed record DivisionAddressDto(
    string? Street,
    string? District,
    string? City,
    string? PostalCode,
    int? CountryId
);

/*
 * Features
  Division
    Commands
      Models
        DivisionAddressInputDto.cs
    Queries
      Models
        DivisionAddressDto.cs
        DivisionDetailsDto.cs
        DivisionListItemDto.cs
*/