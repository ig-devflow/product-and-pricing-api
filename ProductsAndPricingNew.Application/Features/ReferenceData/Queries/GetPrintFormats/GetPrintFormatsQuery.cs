using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetPrintFormats;

public sealed record GetPrintFormatsQuery() : IRequest<Result<IReadOnlyCollection<PrintFormatReferenceDto>>>;