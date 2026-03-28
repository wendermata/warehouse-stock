using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockMovement;

public sealed class GetStockMovementByIdHandler(IStockMovementRepository repo)
{
    public Task<StockMovementEntity?> HandleAsync(Guid id, CancellationToken ct = default)
        => repo.GetByIdAsync(id, ct);
}
