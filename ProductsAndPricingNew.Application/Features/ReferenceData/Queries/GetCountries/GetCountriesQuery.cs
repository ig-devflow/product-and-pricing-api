using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCountries;

public sealed record GetCountriesQuery() : IRequest<Result<IReadOnlyCollection<CountryReferenceDto>>>;
