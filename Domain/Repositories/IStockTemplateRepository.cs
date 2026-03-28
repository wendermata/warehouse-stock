using WarehouseStockService.Domain.Entities;

namespace WarehouseStockService.Domain.Repositories;

public interface IStockTemplateRepository
{
    Task<StockTemplateEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<StockTemplateEntity?> GetByExternalReferenceAsync(string externalReference, CancellationToken ct = default);
    Task<IReadOnlyList<StockTemplateEntity>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(StockTemplateEntity template, CancellationToken ct = default);
}
