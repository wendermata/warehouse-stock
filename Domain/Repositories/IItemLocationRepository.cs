using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.ReadModels;

namespace WarehouseStockService.Domain.Repositories;

public interface IItemLocationRepository
{
    Task<ItemLocationDetail?> GetDetailByIdAsync(Guid id, CancellationToken ct = default);
    Task<ItemLocationEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default);
    Task<ItemLocationEntity?> GetBySkuAndLocationAsync(string sku, Guid stockLocationId, CancellationToken ct = default);
    Task<IReadOnlyList<ItemLocationDetail>> GetDetailsByLocationIdAsync(Guid stockLocationId, CancellationToken ct = default);
    Task<IReadOnlyList<ItemLocationDetail>> GetDetailsBySkuAsync(string sku, CancellationToken ct = default);
    Task AddAsync(ItemLocationEntity itemLocation, CancellationToken ct = default);
    Task UpdateAsync(ItemLocationEntity itemLocation, CancellationToken ct = default);
}
