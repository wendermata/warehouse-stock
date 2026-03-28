using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.ItemLocation;

public sealed record ApplyExitInput(Guid Id, int Quantity, string ExternalReference);

public sealed class ApplyExitHandler(
    IItemLocationRepository repo,
    IStockMovementRepository movementRepo,
    IUnitOfWork uow)
{
    public async Task<ItemLocationOutput> HandleAsync(ApplyExitInput input, CancellationToken ct = default)
    {
        await uow.BeginTransactionAsync(ct);

        var itemLocation = await repo.GetByIdForUpdateAsync(input.Id, ct)
            ?? throw new NotFoundException(nameof(ItemLocationEntity), input.Id);

        itemLocation.ApplyExit(input.Quantity);

        var movement = StockMovementEntity.RegisterExit(
            itemLocation.Id,
            input.Quantity,
            itemLocation.AvailableQuantity,
            input.ExternalReference);

        await repo.UpdateAsync(itemLocation, ct);
        await movementRepo.AddAsync(movement, ct);
        await uow.CommitAsync(ct);

        return new ItemLocationOutput(
            itemLocation.Id,
            itemLocation.StockLocationId,
            itemLocation.Sku,
            itemLocation.AvailableQuantity);
    }
}
