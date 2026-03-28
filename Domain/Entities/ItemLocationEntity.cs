namespace WarehouseStockService.Domain.Entities;

/// <summary>
/// Represents the binding between an item (SKU) and a stock location,
/// tracking the available quantity at that specific location.
/// </summary>
public sealed class ItemLocationEntity
{
    public Guid   Id                { get; private set; }
    public Guid   StockLocationId   { get; private set; }
    public string Sku               { get; private set; } = string.Empty;
    public int    AvailableQuantity { get; private set; }
    public DateTime CreatedAt       { get; private set; }
    public DateTime UpdatedAt       { get; private set; }

    private ItemLocationEntity(Guid id, Guid stockLocationId, string sku, int availableQuantity, DateTime createdAt, DateTime updatedAt)
    {
        Id                = id;
        StockLocationId   = stockLocationId;
        Sku               = sku;
        AvailableQuantity = availableQuantity;
        CreatedAt         = createdAt;
        UpdatedAt         = updatedAt;
    }

    public static ItemLocationEntity Create(Guid stockLocationId, string sku, int initialQuantity = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        if (stockLocationId == Guid.Empty) throw new ArgumentException("StockLocationId must not be empty.", nameof(stockLocationId));
        ArgumentOutOfRangeException.ThrowIfNegative(initialQuantity);

        var now = DateTime.UtcNow;
        return new ItemLocationEntity(Guid.NewGuid(), stockLocationId, sku.Trim().ToUpperInvariant(), initialQuantity, now, now);
    }

    public void ApplyEntry(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        AvailableQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApplyExit(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        if (quantity > AvailableQuantity)
            throw new InvalidOperationException($"Insufficient stock: available={AvailableQuantity}, requested={quantity}.");

        AvailableQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}
