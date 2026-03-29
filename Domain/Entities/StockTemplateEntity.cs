namespace WarehouseStockService.Domain.Entities;

/// <summary>
/// Generic stock template catalogue entry (e.g. A, B, C, D).
/// Shared across branches; each branch binds a physical location to one template.
/// </summary>
public sealed class StockTemplateEntity
{
    public Guid     Id                { get; private set; }
    public string   ExternalReference { get; private set; } = string.Empty;
    public string?  Description       { get; private set; }
    public DateTime  CreatedAt         { get; private set; }
    public DateTime? UpdatedAt         { get; private set; }

    private StockTemplateEntity(
        Guid id,
        string externalReference,
        string? description,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id                = id;
        ExternalReference = externalReference;
        Description       = description;
        CreatedAt         = createdAt;
        UpdatedAt         = updatedAt;
    }

    public static StockTemplateEntity Create(string externalReference, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(externalReference);

        return new StockTemplateEntity(
            Guid.NewGuid(),
            externalReference.Trim().ToUpperInvariant(),
            description,
            DateTime.UtcNow,
            null);
    }
}
