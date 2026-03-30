namespace WarehouseStockService.Domain.ReadModels;

/// <summary>
/// Read model for item location queries that include related stock template data.
/// Not an entity — represents a JOIN projection across item_locations, stock_locations, and stock_templates.
/// </summary>
public sealed record ItemLocationDetail(
    Guid      Id,
    Guid      StockLocationId,
    string    Sku,
    int       AvailableQuantity,
    string?   StockTemplateDescription,
    string    BranchName,
    DateTime  CreatedAt,
    DateTime? UpdatedAt);
