using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockTemplate;

public sealed class GetAllStockTemplatesHandler(IStockTemplateRepository repo)
{
    public Task<IReadOnlyList<StockTemplateEntity>> HandleAsync(CancellationToken ct = default)
        => repo.GetAllAsync(ct);
}
