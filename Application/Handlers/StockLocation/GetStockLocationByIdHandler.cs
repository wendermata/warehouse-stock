using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockLocation;

public sealed class GetStockLocationByIdHandler(IStockLocationRepository repo)
{
    public Task<StockLocationEntity?> HandleAsync(Guid id, CancellationToken ct = default)
        => repo.GetByIdAsync(id, ct);
}
