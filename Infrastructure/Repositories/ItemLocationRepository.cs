using System.Data;
using Dapper;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.ReadModels;
using WarehouseStockService.Domain.Repositories;
using WarehouseStockService.Infrastructure.Persistence;

namespace WarehouseStockService.Infrastructure.Repositories;

internal sealed class ItemLocationRepository(DbSession session) : IItemLocationRepository
{
    public async Task<ItemLocationDetail?> GetDetailByIdAsync(Guid id, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT il.id,
                   il.stock_location_id  AS stockLocationId,
                   il.sku,
                   il.available_quantity AS availableQuantity,
                   st.description        AS stockTemplateDescription,
                   b.name                AS branchName,
                   il.created_at         AS createdAt,
                   il.updated_at         AS updatedAt
            FROM item_locations il
            INNER JOIN stock_locations sl ON sl.id = il.stock_location_id
            INNER JOIN stock_templates st ON st.id = sl.stock_template_id
            INNER JOIN branches b         ON b.id  = sl.branch_id
            WHERE il.id = @id
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<ItemLocationDetail>(
            new CommandDefinition(sql, new { id }, session.Transaction, cancellationToken: ct));
    }

    public async Task<ItemLocationEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id,
                   stock_location_id  AS stockLocationId,
                   sku,
                   available_quantity AS availableQuantity,
                   created_at         AS createdAt,
                   updated_at         AS updatedAt
            FROM item_locations
            WHERE id = @id
            FOR UPDATE
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<ItemLocationEntity>(
            new CommandDefinition(sql, new { id }, session.Transaction, cancellationToken: ct));
    }

    public async Task<ItemLocationEntity?> GetBySkuAndLocationAsync(string sku, Guid stockLocationId, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id,
                   stock_location_id  AS stockLocationId,
                   sku,
                   available_quantity AS availableQuantity,
                   created_at         AS createdAt,
                   updated_at         AS updatedAt
            FROM item_locations
            WHERE sku = @sku AND stock_location_id = @stockLocationId
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<ItemLocationEntity>(
            new CommandDefinition(sql, new { sku, stockLocationId }, session.Transaction, cancellationToken: ct));
    }

    public async Task<IReadOnlyList<ItemLocationDetail>> GetDetailsByLocationIdAsync(Guid stockLocationId, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT il.id,
                   il.stock_location_id  AS stockLocationId,
                   il.sku,
                   il.available_quantity AS availableQuantity,
                   st.description        AS stockTemplateDescription,
                   b.name                AS branchName,
                   il.created_at         AS createdAt,
                   il.updated_at         AS updatedAt
            FROM item_locations il
            INNER JOIN stock_locations sl ON sl.id = il.stock_location_id
            INNER JOIN stock_templates st ON st.id = sl.stock_template_id
            INNER JOIN branches b         ON b.id  = sl.branch_id
            WHERE il.stock_location_id = @stockLocationId
            ORDER BY il.sku
            """;

        var result = await session.Connection.QueryAsync<ItemLocationDetail>(
            new CommandDefinition(sql, new { stockLocationId }, session.Transaction, cancellationToken: ct));

        return result.AsList();
    }

    public async Task<IReadOnlyList<ItemLocationDetail>> GetDetailsBySkuAsync(string sku, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT il.id,
                   il.stock_location_id  AS stockLocationId,
                   il.sku,
                   il.available_quantity AS availableQuantity,
                   st.description        AS stockTemplateDescription,
                   b.name                AS branchName,
                   il.created_at         AS createdAt,
                   il.updated_at         AS updatedAt
            FROM item_locations il
            INNER JOIN stock_locations sl ON sl.id = il.stock_location_id
            INNER JOIN stock_templates st ON st.id = sl.stock_template_id
            INNER JOIN branches b         ON b.id  = sl.branch_id
            WHERE il.sku = @sku
            """;

        var result = await session.Connection.QueryAsync<ItemLocationDetail>(
            new CommandDefinition(sql, new { sku }, session.Transaction, cancellationToken: ct));

        return result.AsList();
    }

    public async Task AddAsync(ItemLocationEntity itemLocation, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            INSERT INTO item_locations (id, stock_location_id, sku, available_quantity, created_at, updated_at)
            VALUES (@Id, @StockLocationId, @Sku, @AvailableQuantity, @CreatedAt, @UpdatedAt)
            """;

        await session.Connection.ExecuteAsync(
            new CommandDefinition(sql, itemLocation, session.Transaction, cancellationToken: ct));
    }

    public async Task UpdateAsync(ItemLocationEntity itemLocation, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            UPDATE item_locations
            SET available_quantity = @AvailableQuantity,
                updated_at         = @UpdatedAt
            WHERE id = @Id
            """;

        await session.Connection.ExecuteAsync(
            new CommandDefinition(sql, itemLocation, session.Transaction, cancellationToken: ct));
    }

    private async Task EnsureOpenAsync(CancellationToken ct)
    {
        if (session.Connection.State == ConnectionState.Closed)
            await session.Connection.OpenAsync(ct);
    }
}
