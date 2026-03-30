using WarehouseStockService.Domain.ReadModels;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.ItemLocation;

public sealed class GetItemLocationByIdHandler(IItemLocationRepository repo)
{
    public Task<ItemLocationDetail?> HandleAsync(Guid id, CancellationToken ct = default)
        => repo.GetDetailByIdAsync(id, ct);
}
