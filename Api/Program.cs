using Scalar.AspNetCore;
using WarehouseStockService.Application;
using WarehouseStockService.Extensions;
using WarehouseStockService.Infrastructure;
using WarehouseStockService.Infrastructure.Database.Migrations;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddEndpoints();
builder.Services.AddOpenApi();

var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionString 'Default' not found.");

MigrationRunner.Run(connectionString, Path.Combine(AppContext.BaseDirectory, "Database", "Scripts"));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapEndpoints();
app.Run();
