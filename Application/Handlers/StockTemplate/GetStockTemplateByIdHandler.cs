using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockTemplate;

public sealed class GetStockTemplateByIdHandler(IStockTemplateRepository repo)
{
    public Task<StockTemplateEntity?> HandleAsync(Guid id, CancellationToken ct = default)
        => repo.GetByIdAsync(id, ct);
}
