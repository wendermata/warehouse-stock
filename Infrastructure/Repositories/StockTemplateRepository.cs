using System.Data;
using Dapper;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;
using WarehouseStockService.Infrastructure.Persistence;

namespace WarehouseStockService.Infrastructure.Repositories;

internal sealed class StockTemplateRepository(DbSession session) : IStockTemplateRepository
{
    public async Task<StockTemplateEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id,
                   external_reference AS externalReference,
                   description,
                   created_at         AS createdAt,
                   updated_at         AS updatedAt
            FROM stock_templates
            WHERE id = @id
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<StockTemplateEntity>(
            new CommandDefinition(sql, new { id }, session.Transaction, cancellationToken: ct));
    }

    public async Task<StockTemplateEntity?> GetByExternalReferenceAsync(string externalReference, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id,
                   external_reference AS externalReference,
                   description,
                   created_at         AS createdAt,
                   updated_at         AS updatedAt
            FROM stock_templates
            WHERE external_reference = @externalReference
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<StockTemplateEntity>(
            new CommandDefinition(sql, new { externalReference }, session.Transaction, cancellationToken: ct));
    }

    public async Task<IReadOnlyList<StockTemplateEntity>> GetAllAsync(CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id,
                   external_reference AS externalReference,
                   description,
                   created_at         AS createdAt,
                   updated_at         AS updatedAt
            FROM stock_templates
            ORDER BY external_reference
            """;

        var result = await session.Connection.QueryAsync<StockTemplateEntity>(
            new CommandDefinition(sql, null, session.Transaction, cancellationToken: ct));

        return result.AsList();
    }

    public async Task AddAsync(StockTemplateEntity template, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            INSERT INTO stock_templates (id, external_reference, description, created_at, updated_at)
            VALUES (@Id, @ExternalReference, @Description, @CreatedAt, @UpdatedAt)
            """;

        await session.Connection.ExecuteAsync(
            new CommandDefinition(sql, template, session.Transaction, cancellationToken: ct));
    }

    private async Task EnsureOpenAsync(CancellationToken ct)
    {
        if (session.Connection.State == ConnectionState.Closed)
            await session.Connection.OpenAsync(ct);
    }
}
