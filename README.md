# ProductApI.Net

Products API built with **Clean Architecture** and **CQRS** on .NET 10.
واجهة Products API مبنية باستخدام Clean Architecture وCQRS.

## Features / المميزات
- `Product` entity with `Id`, `Name`, `Price`
- `GET /api/products` — list all products
- `GET /api/products/{id}` — get a product by id (404 if not found)
- `POST /api/products` — create a product (400 on validation failure)
- Request/response DTOs and validation with FluentValidation
- SQL Server with EF Core, with demo data seeded in Development
- Structured logging with `ILogger` and **Serilog**: console + rolling daily files in `Products.API/logs/`
- Live log viewer in the UI backed by `GET /api/logs` (in-memory Serilog sink)
- Interactive Arabic explorer UI served at `/` (works against the live API, or in-memory demo mode when opened as a file)

## Architecture / المعمارية
```
Products.API            Controllers, middleware, Program.cs, wwwroot UI
Products.Application    Commands, Queries, DTOs, validators, MediatR pipeline
Products.Infrastructure EF Core DbContext, repositories, seeder
Products.Domain         Product entity (no dependencies)
```
Dependencies point inward: `API → Application → Domain`, `Infrastructure → Application → Domain`.

**CQRS** with MediatR:
- Command: `CreateProductCommand`
- Queries: `GetProductsQuery`, `GetProductByIdQuery`
- A `ValidationBehavior` pipeline runs the FluentValidation validators before every handler.

## Employee API — Async/Await & Performance
- `GET/POST /api/employees`, `GET/PUT/DELETE /api/employees/{id}` — all async (`Task<T>`) from Controller to Service to Repository (`ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`), with a `CancellationToken` passed end to end.
- `GET /api/performance/compare?delayA=400&delayB=600` — runs two independent I/O-bound operations sequentially and then in parallel (`Task.WhenAll`), measures each with `Stopwatch` and returns the timings with a short report. Each parallel task uses its own DI scope because `DbContext` is not thread-safe.
- Client-aborted requests are logged as cancelled by the exception middleware instead of surfacing as errors.
- UI: `/employees.html` (CRUD, timeline comparison, cancel button, live server log).
- Note: with an existing SQL Server database that already has the `Products` table, `EnsureCreated` will not add the new `Employees` table; drop the database or use migrations.

## Logging
- `LoggingBehavior` (MediatR pipeline) logs every command/query with its duration; handlers log business events (created, retrieved, not found); the exception middleware logs validation failures and unhandled errors.
- Serilog sinks: Console, rolling File (`logs/products-YYYYMMDD.log`, 14 days), and an in-memory sink exposed at `GET /api/logs?after={id}` for the UI.
- Levels are configured under `Serilog:MinimumLevel` in `appsettings.json`.

## Validation rules
- `Name`: required, max 200 characters
- `Price`: greater than 0

## Getting started / التشغيل
1. Install the .NET 10 SDK and have a SQL Server instance available.
2. Set `ConnectionStrings:DefaultConnection` in `Products.API/appsettings.json`.
3. Run:
   ```bash
   dotnet run --project Products.API
   ```
   No SQL Server? Set `"UseInMemoryDatabase": true` (in `appsettings.Development.json`) to run with an in-memory EF database, or start SQL Server with Docker:
   ```bash
   docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD='Your_password123' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
   ```
4. In Development the database is created and seeded with sample products on startup. Open `/` for the explorer UI, or call `GET /api/products`.

### Example
```http
POST /api/products
Content-Type: application/json

{ "name": "Laptop", "price": 1499.00 }
```
