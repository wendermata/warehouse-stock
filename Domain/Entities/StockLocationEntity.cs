namespace WarehouseStockService.Domain.Entities;

/// <summary>
/// Represents a physical stock location (slot/shelf/area) inside a branch,
/// bound to a specific stock template (e.g. A, B, C).
/// </summary>
public sealed class StockLocationEntity
{
    public Guid     Id              { get; private set; }
    public Guid     BranchId        { get; private set; }
    public Guid     StockTemplateId { get; private set; }
    public DateTime CreatedAt       { get; private set; }
    public DateTime UpdatedAt       { get; private set; }

    private StockLocationEntity(
        Guid id,
        Guid branchId,
        Guid stockTemplateId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id              = id;
        BranchId        = branchId;
        StockTemplateId = stockTemplateId;
        CreatedAt       = createdAt;
        UpdatedAt       = updatedAt;
    }

    public static StockLocationEntity Create(Guid branchId, Guid stockTemplateId)
    {
        if (branchId == Guid.Empty)
            throw new ArgumentException("BranchId must not be empty.", nameof(branchId));
        if (stockTemplateId == Guid.Empty)
            throw new ArgumentException("StockTemplateId must not be empty.", nameof(stockTemplateId));

        var now = DateTime.UtcNow;
        return new StockLocationEntity(Guid.NewGuid(), branchId, stockTemplateId, now, now);
    }
}
