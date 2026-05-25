namespace ProductsAndPricingNew.Application.Features.Transfer.Abstractions;

public interface ITransferQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
}