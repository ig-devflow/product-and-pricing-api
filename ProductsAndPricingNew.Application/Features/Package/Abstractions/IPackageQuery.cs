namespace ProductsAndPricingNew.Application.Features.Package.Abstractions;

public interface IPackageQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
}