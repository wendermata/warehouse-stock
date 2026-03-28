using WarehouseStockService.Domain.Entities;

namespace WarehouseStockService.Domain.Repositories;

public interface IItemLocationRepository
{
    Task<ItemLocationEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ItemLocationEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default);
    Task<ItemLocationEntity?> GetBySkuAndLocationAsync(string sku, Guid stockLocationId, CancellationToken ct = default);
    Task<IReadOnlyList<ItemLocationEntity>> GetByLocationIdAsync(Guid stockLocationId, CancellationToken ct = default);
    Task<IReadOnlyList<ItemLocationEntity>> GetBySkuAsync(string sku, CancellationToken ct = default);
    Task AddAsync(ItemLocationEntity itemLocation, CancellationToken ct = default);
    Task UpdateAsync(ItemLocationEntity itemLocation, CancellationToken ct = default);
}
