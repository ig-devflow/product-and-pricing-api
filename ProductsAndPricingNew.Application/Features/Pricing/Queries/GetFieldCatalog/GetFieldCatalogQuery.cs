using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Pricing.Models;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Application.Features.Pricing.Queries.GetFieldCatalog;

public sealed record GetFieldCatalogQuery(RuleSubjectType Subject)
    : IRequest<Result<IReadOnlyCollection<FieldDescriptorDto>>>;
