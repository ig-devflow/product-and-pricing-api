using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Domain.Abstractions;

public interface IHasAuditMetadata
{
    AuditMetadata AuditMetadata { get; }
}