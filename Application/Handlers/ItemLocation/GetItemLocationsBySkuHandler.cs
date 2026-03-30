using WarehouseStockService.Domain.ReadModels;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.ItemLocation;

public sealed class GetItemLocationsBySkuHandler(IItemLocationRepository repo)
{
    public Task<IReadOnlyList<ItemLocationDetail>> HandleAsync(string sku, CancellationToken ct = default)
        => repo.GetDetailsBySkuAsync(sku, ct);
}
