using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.Branch;

public sealed class GetAllBranchesHandler(IBranchRepository repo)
{
    public Task<IReadOnlyList<BranchEntity>> HandleAsync(CancellationToken ct = default)
        => repo.GetAllAsync(ct);
}
