using DbUp;

namespace WarehouseStockService.Infrastructure.Database.Migrations;

public static class MigrationRunner
{
    public static void Run(string connectionString, string scriptsPath)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem(scriptsPath)
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
            throw result.Error;
    }
}
