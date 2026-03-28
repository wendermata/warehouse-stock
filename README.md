# Warehouse Stock

A **Proof of Concept** of an ASP.NET Core Minimal API targeting **Native AOT** compilation for maximum startup performance and reduced memory footprint. The domain models a warehouse stock control system built around an **immutable ledger** pattern for stock movements.

## Goals

- Explore Native AOT with Minimal API in a realistic domain scenario
- Validate Clean Architecture layering (Domain / Application / Infrastructure / Api) under AOT constraints
- Benchmark cold-start times and memory usage compared to a JIT-compiled equivalent

## Tech Stack

| Concern | Library / Tool |
|---|---|
| Framework | ASP.NET Core 10 — Minimal API (`WebApplication.CreateSlimBuilder`) |
| ORM | [Dapper](https://github.com/DapperLib/Dapper) — lightweight micro-ORM |
| Database | PostgreSQL 14+ via [Npgsql](https://www.npgsql.org/) |
| Migrations | [DbUp](https://dbup.readthedocs.io/) — script-based, runs on startup |
| Messaging | [RabbitMQ.Client 7](https://www.rabbitmq.com/client-libraries/dotnet) — async-first API |
| API Docs | [Scalar](https://scalar.com/) — OpenAPI UI (development only) |

## Architecture

```
Api/                   # Minimal API endpoints + Program.cs
Application/           # Use-case handlers, domain ports (IMessagePublisher), events
Domain/                # Entities, enums, repository interfaces — zero external dependencies
Infrastructure/        # Dapper repositories, RabbitMQ publisher, DbUp migration runner
```

Dependency flow: `Api → Application → Domain` and `Infrastructure → Application → Domain`.
The `Api` project wires everything together via DI.

## Domain Model

```
Branch (1) ──→ (N) StockLocation ←── (1) StockTemplate
                       │
                    (1) ↓ (N)
                  ItemLocation (SKU + quantity per location)
                       │
                    (1) ↓ (N)
                 StockMovement  ← immutable ledger record
```

Every stock **entry** or **exit** is recorded as an immutable `StockMovement` carrying the resulting balance (`balance_after`). The current quantity of an `ItemLocation` is always the balance of its latest movement.

The ledger also acts as the **concurrency control mechanism**: stock entry and exit operations acquire a pessimistic row-level lock (`SELECT ... FOR UPDATE`) on the `ItemLocation` row before applying the movement. This prevents race conditions where concurrent requests could read the same stale balance and produce an inconsistent result.

## Messaging

After each successful stock movement (entry or exit), an event is published to RabbitMQ:

- **Exchange:** `warehouse-stock` (topic, durable)
- **Routing keys:** `stock.movement.entry` / `stock.movement.exit`
- **Payload:** `StockMovementCreatedEvent` — movement ID, item location ID, type, quantity, balance after, external reference, timestamp

## API Endpoints

| Method | Path | Description |
|---|---|---|
| `GET` | `/branches` | List all branches |
| `GET` | `/branches/{id}` | Get branch by ID |
| `POST` | `/branches` | Create branch |
| `PUT` | `/branches/{id}` | Rename branch |
| `GET` | `/stock-templates` | List stock templates |
| `GET` | `/stock-templates/{id}` | Get template by ID |
| `POST` | `/stock-templates` | Create stock template |
| `GET` | `/stock-locations?branchId=` | List locations by branch |
| `GET` | `/stock-locations/{id}` | Get location by ID |
| `POST` | `/stock-locations` | Create stock location |
| `GET` | `/item-locations/{id}` | Get item location by ID |
| `GET` | `/item-locations/by-location?stockLocationId=` | List items at a location |
| `GET` | `/item-locations/by-sku?sku=` | Find item locations by SKU |
| `POST` | `/item-locations` | Create item location |
| `POST` | `/item-locations/{id}/entry` | Apply stock entry |
| `POST` | `/item-locations/{id}/exit` | Apply stock exit |
| `GET` | `/stock-movements/{id}` | Get movement by ID |
| `GET` | `/stock-movements/by-item?itemLocationId=&limit=&offset=` | List movements (paginated) |

Interactive docs available at `/scalar/v1` when running in Development mode.

## Configuration

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=warehouse_stock;Username=postgres;Password=..."
  },
  "RabbitMq": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "ExchangeName": "warehouse-stock"
  }
}
```

## Running Locally

**Prerequisites:** .NET 10 SDK, PostgreSQL, RabbitMQ.

```bash
# Clone and run — migrations execute automatically on startup
dotnet run --project Api
```
