using WarehouseStockService.Domain.Entities;

namespace WarehouseStockService.Domain.Repositories;

public interface IStockMovementRepository
{
    Task<StockMovementEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<StockMovementEntity>> GetByItemLocationIdAsync(Guid itemLocationId, int limit = 50, int offset = 0, CancellationToken ct = default);
    Task AddAsync(StockMovementEntity movement, CancellationToken ct = default);
}
