using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.ItemLocation;

public sealed class GetItemLocationsByLocationHandler(IItemLocationRepository repo)
{
    public Task<IReadOnlyList<ItemLocationEntity>> HandleAsync(Guid stockLocationId, CancellationToken ct = default)
        => repo.GetByLocationIdAsync(stockLocationId, ct);
}
