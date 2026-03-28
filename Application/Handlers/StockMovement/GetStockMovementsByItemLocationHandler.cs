using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockMovement;

public sealed record GetStockMovementsByItemLocationInput(Guid ItemLocationId, int Limit = 50, int Offset = 0);

public sealed class GetStockMovementsByItemLocationHandler(IStockMovementRepository repo)
{
    public Task<IReadOnlyList<StockMovementEntity>> HandleAsync(GetStockMovementsByItemLocationInput input, CancellationToken ct = default)
        => repo.GetByItemLocationIdAsync(input.ItemLocationId, input.Limit, input.Offset, ct);
}
