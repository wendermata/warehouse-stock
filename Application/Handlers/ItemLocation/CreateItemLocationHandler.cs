using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.ItemLocation;

public sealed record CreateItemLocationInput(Guid StockLocationId, string Sku, int InitialQuantity = 0);
public sealed record CreateItemLocationOutput(Guid Id, Guid StockLocationId, string Sku, int AvailableQuantity);

public sealed class CreateItemLocationHandler(
    IItemLocationRepository repo,
    IStockLocationRepository locationRepo,
    IUnitOfWork uow)
{
    public async Task<CreateItemLocationOutput> HandleAsync(CreateItemLocationInput input, CancellationToken ct = default)
    {
        var location = await locationRepo.GetByIdAsync(input.StockLocationId, ct)
            ?? throw new NotFoundException(nameof(StockLocationEntity), input.StockLocationId);

        var existing = await repo.GetBySkuAndLocationAsync(input.Sku, input.StockLocationId, ct);
        if (existing is not null)
            throw new ConflictException($"Item '{input.Sku}' already exists in location '{location.Id}'.");

        var itemLocation = ItemLocationEntity.Create(input.StockLocationId, input.Sku, input.InitialQuantity);

        await uow.BeginTransactionAsync(ct);
        await repo.AddAsync(itemLocation, ct);
        await uow.CommitAsync(ct);

        return new CreateItemLocationOutput(
            itemLocation.Id,
            itemLocation.StockLocationId,
            itemLocation.Sku,
            itemLocation.AvailableQuantity);
    }
}
