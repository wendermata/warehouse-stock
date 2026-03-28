using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.Branch;

public sealed class GetBranchByIdHandler(IBranchRepository repo)
{
    public Task<BranchEntity?> HandleAsync(Guid id, CancellationToken ct = default)
        => repo.GetByIdAsync(id, ct);
}
