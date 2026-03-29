using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.ReadModels;

namespace WarehouseStockService.Domain.Repositories;

public interface IStockLocationRepository
{
    Task<StockLocationDetail?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<StockLocationEntity?> GetByTemplateAndBranchAsync(Guid branchId, Guid stockTemplateId, CancellationToken ct = default);
    Task<IReadOnlyList<StockLocationDetail>> GetByBranchIdAsync(Guid branchId, CancellationToken ct = default);
    Task AddAsync(StockLocationEntity stockLocation, CancellationToken ct = default);
}
