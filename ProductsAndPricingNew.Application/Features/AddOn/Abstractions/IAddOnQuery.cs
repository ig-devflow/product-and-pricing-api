namespace ProductsAndPricingNew.Application.Features.AddOn.Abstractions;

public interface IAddOnQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
}