using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.ItemLocation;

public sealed class GetItemLocationsBySkuHandler(IItemLocationRepository repo)
{
    public Task<IReadOnlyList<ItemLocationEntity>> HandleAsync(string sku, CancellationToken ct = default)
        => repo.GetBySkuAsync(sku, ct);
}
