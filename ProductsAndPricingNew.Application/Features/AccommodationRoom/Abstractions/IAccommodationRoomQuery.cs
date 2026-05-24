namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;

public interface IAccommodationRoomQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
}