using System.Data;
using Dapper;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;
using WarehouseStockService.Infrastructure.Persistence;

namespace WarehouseStockService.Infrastructure.Repositories;

internal sealed class BranchRepository(DbSession session) : IBranchRepository
{
    public async Task<BranchEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id, code, name, created_at AS createdAt
            FROM branches
            WHERE id = @id
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<BranchEntity>(
            new CommandDefinition(sql, new { id }, session.Transaction, cancellationToken: ct));
    }

    public async Task<BranchEntity?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id, code, name, created_at AS createdAt
            FROM branches
            WHERE code = @code
            """;

        return await session.Connection.QueryFirstOrDefaultAsync<BranchEntity>(
            new CommandDefinition(sql, new { code }, session.Transaction, cancellationToken: ct));
    }

    public async Task<IReadOnlyList<BranchEntity>> GetAllAsync(CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            SELECT id, code, name, created_at AS createdAt
            FROM branches
            ORDER BY code
            """;

        var result = await session.Connection.QueryAsync<BranchEntity>(
            new CommandDefinition(sql, transaction: session.Transaction, cancellationToken: ct));

        return result.AsList();
    }

    public async Task AddAsync(BranchEntity branch, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            INSERT INTO branches (id, code, name, created_at)
            VALUES (@Id, @Code, @Name, @CreatedAt)
            """;

        await session.Connection.ExecuteAsync(
            new CommandDefinition(sql, branch, session.Transaction, cancellationToken: ct));
    }

    public async Task UpdateAsync(BranchEntity branch, CancellationToken ct = default)
    {
        await EnsureOpenAsync(ct);

        const string sql = """
            UPDATE branches
            SET name = @Name
            WHERE id = @Id
            """;

        await session.Connection.ExecuteAsync(
            new CommandDefinition(sql, branch, session.Transaction, cancellationToken: ct));
    }

    private async Task EnsureOpenAsync(CancellationToken ct)
    {
        if (session.Connection.State == ConnectionState.Closed)
            await session.Connection.OpenAsync(ct);
    }
}
