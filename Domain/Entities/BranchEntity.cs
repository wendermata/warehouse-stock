namespace WarehouseStockService.Domain.Entities;

public sealed class BranchEntity
{
    public Guid   Id        { get; private set; }
    public string Code      { get; private set; } = string.Empty;
    public string Name      { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private BranchEntity(Guid id, string code, string name, DateTime createdAt)
    {
        Id        = id;
        Code      = code;
        Name      = name;
        CreatedAt = createdAt;
    }

    public static BranchEntity Create(string code, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new BranchEntity(Guid.NewGuid(), code.Trim().ToUpperInvariant(), name.Trim(), DateTime.UtcNow);
    }

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
    }
}
