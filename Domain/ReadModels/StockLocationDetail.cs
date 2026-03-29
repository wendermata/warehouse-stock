namespace WarehouseStockService.Domain.ReadModels;

/// <summary>
/// Read model for stock location queries that include related stock template data.
/// Not an entity — represents a JOIN projection, not a single table row.
/// </summary>
public sealed record StockLocationDetail(
    Guid      Id,
    Guid      BranchId,
    Guid      StockTemplateId,
    string?   StockTemplateDescription,
    DateTime  CreatedAt,
    DateTime? UpdatedAt);
