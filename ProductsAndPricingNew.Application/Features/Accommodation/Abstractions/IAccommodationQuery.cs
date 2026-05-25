namespace ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;

public interface IAccommodationQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
}