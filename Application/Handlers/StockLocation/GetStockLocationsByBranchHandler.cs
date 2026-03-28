using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockLocation;

public sealed class GetStockLocationsByBranchHandler(IStockLocationRepository repo)
{
    public Task<IReadOnlyList<StockLocationEntity>> HandleAsync(Guid branchId, CancellationToken ct = default)
        => repo.GetByBranchIdAsync(branchId, ct);
}
