using System.Data;
using Dapper;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;
using WarehouseStockService.Infrastructure.Persistence;

namespace WarehouseStockService.Infrastructure.Repositories;

internal sealed class StockMovementRepository(DbSession session) : IStockMovementRepository
{
    public async Task<StockMovementEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id,
                   item_location_id AS itemLocationId,
                   type,
                   quantity,
                   balance_after    AS balanceAfter,
                   external_reference AS externalReference,
                   occurred_at      AS occurredAt
            FROM stock_movements
            WHERE id = @id
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<StockMovementEntity>(
            new CommandDefinition(sql, new { id }, session.Transaction, cancellationToken: ct));
    }

    public async Task<IReadOnlyList<StockMovementEntity>> GetByItemLocationIdAsync(
        Guid itemLocationId,
        int limit = 50,
        int offset = 0,
        CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id,
                   item_location_id AS itemLocationId,
                   type,
                   quantity,
                   balance_after    AS balanceAfter,
                   external_reference AS externalReference,
                   occurred_at      AS occurredAt
            FROM stock_movements
            WHERE item_location_id = @itemLocationId
            ORDER BY occurred_at DESC
            LIMIT @limit OFFSET @offset
            """;

        var result = await session.Connection.QueryAsync<StockMovementEntity>(
            new CommandDefinition(sql, new { itemLocationId, limit, offset }, session.Transaction, cancellationToken: ct));

        return result.AsList();
    }

    public async Task<bool> ExistsAsync(Guid itemLocationId, string externalReference, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT EXISTS (
                SELECT 1 FROM stock_movements
                WHERE item_location_id  = @itemLocationId
                  AND external_reference = @externalReference
            )
            """;

        return await session.Connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(sql, new { itemLocationId, externalReference }, session.Transaction, cancellationToken: ct));
    }

    public async Task AddAsync(StockMovementEntity movement, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            INSERT INTO stock_movements (id, item_location_id, type, quantity, balance_after, external_reference, occurred_at)
            VALUES (@Id, @ItemLocationId, @Type, @Quantity, @BalanceAfter, @ExternalReference, @OccurredAt)
            """;

        await session.Connection.ExecuteAsync(
            new CommandDefinition(sql, movement, session.Transaction, cancellationToken: ct));
    }

    private async Task EnsureOpenAsync(CancellationToken ct)
    {
        if (session.Connection.State == ConnectionState.Closed)
            await session.Connection.OpenAsync(ct);
    }
}
