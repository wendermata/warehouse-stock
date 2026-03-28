using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.ItemLocation;

public sealed class GetItemLocationByIdHandler(IItemLocationRepository repo)
{
    public Task<ItemLocationEntity?> HandleAsync(Guid id, CancellationToken ct = default)
        => repo.GetByIdAsync(id, ct);
}
