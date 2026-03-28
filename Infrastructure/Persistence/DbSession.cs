using Npgsql;

namespace WarehouseStockService.Infrastructure.Persistence;

internal sealed class DbSession(string connectionString) : IAsyncDisposable
{
    public NpgsqlConnection Connection { get; } = new(connectionString);
    public NpgsqlTransaction? Transaction { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (Transaction is not null)
            await Transaction.DisposeAsync();

        await Connection.DisposeAsync();
    }
}
