using WarehouseStockService.Domain.ReadModels;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.ItemLocation;

public sealed class GetItemLocationsByLocationHandler(IItemLocationRepository repo)
{
    public Task<IReadOnlyList<ItemLocationDetail>> HandleAsync(Guid stockLocationId, CancellationToken ct = default)
        => repo.GetDetailsByLocationIdAsync(stockLocationId, ct);
}
