using WarehouseStockService.Domain.ReadModels;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockLocation;

public sealed class GetStockLocationByIdHandler(IStockLocationRepository repo)
{
    public Task<StockLocationDetail?> HandleAsync(Guid id, CancellationToken ct = default)
        => repo.GetByIdAsync(id, ct);
}
