using WarehouseStockService.Domain.Enums;

namespace WarehouseStockService.Domain.Entities;

/// <summary>
/// Immutable ledger record for every stock movement (entry or exit).
/// Each record is append-only and carries the resulting balance after the operation.
/// </summary>
public sealed class StockMovementEntity
{
    public Guid         Id             { get; private set; }
    public Guid         ItemLocationId { get; private set; }
    public MovementType Type           { get; private set; }

    /// <summary>Always positive — represents the absolute quantity moved.</summary>
    public int Quantity { get; private set; }

    /// <summary>Resulting balance of the ItemLocation after this movement.</summary>
    public int BalanceAfter { get; private set; }

    /// <summary>External reference (order number, transfer note, etc.).</summary>
    public string ExternalReference { get; private set; } = string.Empty;

    public DateTime OccurredAt { get; private set; }

    private StockMovementEntity(
        Guid id,
        Guid itemLocationId,
        MovementType type,
        int quantity,
        int balanceAfter,
        string externalReference,
        DateTime occurredAt)
    {
        Id                = id;
        ItemLocationId    = itemLocationId;
        Type              = type;
        Quantity          = quantity;
        BalanceAfter      = balanceAfter;
        ExternalReference = externalReference;
        OccurredAt        = occurredAt;
    }

    public static StockMovementEntity RegisterEntry(
        Guid itemLocationId,
        int quantity,
        int balanceAfter,
        string externalReference)
    {
        if (itemLocationId == Guid.Empty) throw new ArgumentException("ItemLocationId must not be empty.", nameof(itemLocationId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(balanceAfter);
        ArgumentException.ThrowIfNullOrWhiteSpace(externalReference);

        return new StockMovementEntity(Guid.NewGuid(), itemLocationId, MovementType.Entry, quantity, balanceAfter, externalReference, DateTime.UtcNow);
    }

    public static StockMovementEntity RegisterExit(
        Guid itemLocationId,
        int quantity,
        int balanceAfter,
        string externalReference)
    {
        if (itemLocationId == Guid.Empty) throw new ArgumentException("ItemLocationId must not be empty.", nameof(itemLocationId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(balanceAfter);
        ArgumentException.ThrowIfNullOrWhiteSpace(externalReference);

        return new StockMovementEntity(Guid.NewGuid(), itemLocationId, MovementType.Exit, quantity, balanceAfter, externalReference, DateTime.UtcNow);
    }
}
