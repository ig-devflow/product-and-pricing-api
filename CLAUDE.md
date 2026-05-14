# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Restore (locked mode required)
dotnet restore ProductsAndPricingNew.sln --locked-mode

# Format check
dotnet format ProductsAndPricingNew.sln --verify-no-changes --severity error --no-restore

# Build
dotnet build ProductsAndPricingNew.sln --configuration Release --no-restore

# Run all tests
dotnet test ProductsAndPricingNew.sln --configuration Release --no-restore

# Run a single test project
dotnet test ProductsAndPricingNew.UnitTests --configuration Release --no-restore

# Run a specific test by name filter
dotnet test ProductsAndPricingNew.sln --filter "FullyQualifiedName~MyTestName" --no-restore
```

## Architecture

.NET 10 ASP.NET Core Web API using Clean Architecture + DDD. Four source projects:

- **AdminApi** — controllers, request contracts, AutoMapper profiles, HTTP result mapping, exception middleware
- **Application** — MediatR commands/queries, handlers, FluentValidation validators, DTOs, pagination
- **Domain** — entities, value objects, domain rules, repository interfaces, `IAuditable`/`ISoftDeletable` abstractions
- **Persistence** — EF Core 10 (SQL Server) DbContext + configurations, Dapper read queries, repositories, audit/soft-delete interceptors, unit of work

Four test projects: `UnitTests`, `IntegrationTests`, `ArchitectureTests`, `Api.E2ETests`.

## Layering Rules

- Domain has zero dependencies on Application, Persistence, or API layers (enforced by `ArchitectureTests`).
- Business invariants belong on domain entities/value objects, not handlers.
- MediatR handlers in Application layer own use-case orchestration.
- Handlers return `Result<T>` (FluentResults) — never throw for expected failures.
- Aggregate writes go through EF repositories + `IUnitOfWork.SaveChangesAsync()`.
- Complex read-side queries use Dapper query classes (see `Queries/` in Persistence), not EF.

## Adding a Feature

Follow the Division feature as the canonical pattern:

**Application layer** (`Features/{Feature}/`):
- `Commands/{Action}/` — command record, handler (`Result<T>`), validator
- `Queries/{Action}/` — query record, handler
- `Models/` — DTOs

**Persistence layer:**
- Repository implements `EfRepositoryBase<TAggregate, TId>` and the domain interface
- Complex reads use a Dapper query class; both are auto-registered by Scrutor (suffix `Repository` or `Query`)

**AdminApi layer:**
- Controller maps requests via AutoMapper → command/query, dispatches via MediatR
- Call `.ToActionResult()` on `Result<T>` for consistent HTTP responses (200/201/400/409/500)
- Add request contracts under `Contracts/{Feature}/`

**EF configuration** goes in `Persistence/Configuration/{Feature}/`, registered automatically via `ConfigurationExtensions`.

## Key Patterns

**Audit & soft delete** — entities implement `IAuditable` (populated by `AuditSaveChangesInterceptor`) and/or `ISoftDeletable` (filtered by `SoftDeleteInterceptor`). Use `ConfigureAuditMetadata()` extension in EF configuration.

**Value objects** — complex value objects (Address, ImageFile, AgeRange, AuditMetadata) are owned as EF `ComplexProperty`. Strongly-typed primitives (EmailAddress, TelephoneNumber, HexColor) live in `Domain/ValueObjects`.

**Entity builders** — domain entities use an inner `Builder` class for construction (see `Centre.Builder`). This enforces invariants at creation time.

**Error handling** — `GlobalExceptionHandler` maps: `DomainException` → 400, `DbUpdateConcurrencyException` → 409, unhandled → 500.

## Coding Conventions

- Prefer explicit types over `var` (enforced by `.editorconfig`).
- Nullable reference types are enabled globally (`Directory.Build.props`).
- Implicit usings are enabled; do not add redundant `using` statements for BCL types.
- Packages are locked — run `dotnet restore --locked-mode`; do not add packages without updating the lock file.
- Do not add EF migrations unless the task explicitly asks for them.

## Database

SQL Server Express, connection string in `appsettings.Development.json`:
```
server=.\SQLEXPRESS; database=ProductsAndPricing_FromScratch; trusted_connection=true; TrustServerCertificate=True
```

Migrations are in `ProductsAndPricingNew.Persistence/Migrations/`.
