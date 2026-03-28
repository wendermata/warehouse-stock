using System.Data;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Infrastructure.Persistence;

internal sealed class UnitOfWork(DbSession session) : IUnitOfWork
{
    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (session.Connection.State == ConnectionState.Closed)
            await session.Connection.OpenAsync(ct);

        session.Transaction = await session.Connection.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        await session.Transaction!.CommitAsync(ct);
        await session.Transaction.DisposeAsync();
        session.Transaction = null;
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        await session.Transaction!.RollbackAsync(ct);
        await session.Transaction.DisposeAsync();
        session.Transaction = null;
    }

    public ValueTask DisposeAsync() => session.DisposeAsync();
}
