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
4. In Development the database is created and seeded with sample products on startup. Open `/` for the explorer UI, or call `GET /api/products`.

### Example
```http
POST /api/products
Content-Type: application/json

{ "name": "Laptop", "price": 1499.00 }
```
