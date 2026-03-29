using WarehouseStockService.Domain.ReadModels;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockLocation;

public sealed class GetStockLocationsByBranchHandler(IStockLocationRepository repo)
{
    public Task<IReadOnlyList<StockLocationDetail>> HandleAsync(Guid branchId, CancellationToken ct = default)
        => repo.GetByBranchIdAsync(branchId, ct);
}
