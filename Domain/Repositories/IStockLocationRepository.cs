using WarehouseStockService.Domain.Entities;

namespace WarehouseStockService.Domain.Repositories;

public interface IStockLocationRepository
{
    Task<StockLocationEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<StockLocationEntity?> GetByTemplateAndBranchAsync(Guid branchId, Guid stockTemplateId, CancellationToken ct = default);
    Task<IReadOnlyList<StockLocationEntity>> GetByBranchIdAsync(Guid branchId, CancellationToken ct = default);
    Task AddAsync(StockLocationEntity stockLocation, CancellationToken ct = default);
}
