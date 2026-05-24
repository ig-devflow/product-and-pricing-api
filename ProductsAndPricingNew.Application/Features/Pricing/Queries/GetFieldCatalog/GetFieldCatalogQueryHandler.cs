using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Pricing.Models;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

namespace ProductsAndPricingNew.Application.Features.Pricing.Queries.GetFieldCatalog;

internal sealed class GetFieldCatalogQueryHandler : IRequestHandler<GetFieldCatalogQuery, Result<IReadOnlyCollection<FieldDescriptorDto>>>
{
    private readonly IFieldCatalog _fieldCatalog;

    public GetFieldCatalogQueryHandler(IFieldCatalog fieldCatalog)
    {
        _fieldCatalog = fieldCatalog;
    }

    public Task<Result<IReadOnlyCollection<FieldDescriptorDto>>> Handle(GetFieldCatalogQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<FieldDescriptorDto> fields = _fieldCatalog
            .GetFields(request.Subject)
            .Select(Map)
            .ToList();

        return Task.FromResult(Result.Ok(fields));
    }

    private static FieldDescriptorDto Map(FieldDescriptor field) => new(
        field.Key,
        field.DataType.ToString(),
        field.Label,
        field.AllowedOperators.Select(op => op.ToString()).ToList(),
        field.ValueSource?.ToString(),
        field.EnumValues);
}
