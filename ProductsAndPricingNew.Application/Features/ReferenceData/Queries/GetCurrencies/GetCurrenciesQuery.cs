using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCurrencies;

public sealed record GetCurrenciesQuery() : IRequest<Result<IReadOnlyCollection<CurrencyReferenceDto>>>;
