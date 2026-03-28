namespace WarehouseStockService.Application.Events;

public sealed record StockMovementCreatedEvent(
    Guid MovementId,
    Guid ItemLocationId,
    string MovementType,
    int Quantity,
    int BalanceAfter,
    string ExternalReference,
    DateTime OccurredAt);
