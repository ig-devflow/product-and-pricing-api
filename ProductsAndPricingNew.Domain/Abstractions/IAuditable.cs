using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Domain.Abstractions;

public interface IAuditable
{
    AuditMetadata AuditMetadata { get; }
}