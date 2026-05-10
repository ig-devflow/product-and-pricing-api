using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetContentTemplates;

public sealed record GetContentTemplatesQuery(
    ContentTemplateScope? Scope = null
) : IRequest<Result<IReadOnlyCollection<ContentTemplateReferenceDto>>>;
