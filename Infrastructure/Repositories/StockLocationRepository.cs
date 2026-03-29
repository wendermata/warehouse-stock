using System.Data;
using Dapper;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.ReadModels;
using WarehouseStockService.Domain.Repositories;
using WarehouseStockService.Infrastructure.Persistence;

namespace WarehouseStockService.Infrastructure.Repositories;

internal sealed class StockLocationRepository(DbSession session) : IStockLocationRepository
{
    public async Task<StockLocationDetail?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT sl.id,
                   sl.branch_id         AS branchId,
                   sl.stock_template_id AS stockTemplateId,
                   st.description       AS stockTemplateDescription,
                   sl.created_at        AS createdAt,
                   sl.updated_at        AS updatedAt
            FROM stock_locations sl
            JOIN stock_templates st ON st.id = sl.stock_template_id
            WHERE sl.id = @id
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<StockLocationDetail>(
            new CommandDefinition(sql, new { id }, session.Transaction, cancellationToken: ct));
    }

    public async Task<StockLocationEntity?> GetByTemplateAndBranchAsync(Guid branchId, Guid stockTemplateId, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id,
                   branch_id         AS branchId,
                   stock_template_id AS stockTemplateId,
                   created_at        AS createdAt,
                   updated_at        AS updatedAt
            FROM stock_locations
            WHERE branch_id = @branchId AND stock_template_id = @stockTemplateId
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<StockLocationEntity>(
            new CommandDefinition(sql, new { branchId, stockTemplateId }, session.Transaction, cancellationToken: ct));
    }

    public async Task<IReadOnlyList<StockLocationDetail>> GetByBranchIdAsync(Guid branchId, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT sl.id,
                   sl.branch_id         AS branchId,
                   sl.stock_template_id AS stockTemplateId,
                   st.description       AS stockTemplateDescription,
                   sl.created_at        AS createdAt,
                   sl.updated_at        AS updatedAt
            FROM stock_locations sl
            JOIN stock_templates st ON st.id = sl.stock_template_id
            WHERE sl.branch_id = @branchId
            ORDER BY st.external_reference
            """;

        var result = await session.Connection.QueryAsync<StockLocationDetail>(
            new CommandDefinition(sql, new { branchId }, session.Transaction, cancellationToken: ct));

        return result.AsList();
    }

    public async Task AddAsync(StockLocationEntity stockLocation, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            INSERT INTO stock_locations (id, branch_id, stock_template_id, created_at, updated_at)
            VALUES (@Id, @BranchId, @StockTemplateId, @CreatedAt, @UpdatedAt)
            """;

        await session.Connection.ExecuteAsync(
            new CommandDefinition(sql, stockLocation, session.Transaction, cancellationToken: ct));
    }

    private async Task EnsureOpenAsync(CancellationToken ct)
    {
        if (session.Connection.State == ConnectionState.Closed)
            await session.Connection.OpenAsync(ct);
    }
}
