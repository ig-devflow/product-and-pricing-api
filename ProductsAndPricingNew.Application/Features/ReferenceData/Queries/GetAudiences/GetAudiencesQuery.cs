using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAudiences;

public sealed record GetAudiencesQuery() : IRequest<Result<IReadOnlyCollection<AudienceReferenceDto>>>;
