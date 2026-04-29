# AGENTS.md

## Scope

This is the backend .NET 8 solution for ProductsAndPricingNew. Work from the repository root unless a task says otherwise. Do not modify generated `bin/` or `obj/` output.

## Architecture

- `ProductsAndPricingNew.AdminApi`: ASP.NET Core Web API, controllers, request contracts, HTTP result mapping, middleware, and API infrastructure.
- `ProductsAndPricingNew.Application`: MediatR commands/queries, handlers, validators, DTOs, pagination, and application errors.
- `ProductsAndPricingNew.Domain`: entities, value objects, domain rules, repository interfaces, audit/soft-delete abstractions.
- `ProductsAndPricingNew.Persistence`: EF Core SQL Server context/configurations/repositories, Dapper read queries, unit of work, audit and soft-delete interceptors.
- Tests live in `ProductsAndPricingNew.UnitTests`, `ProductsAndPricingNew.IntegrationTests`, `ProductsAndPricingNew.ArchitectureTests`, and `ProductsAndPricingNew.Api.E2ETests`.

## Layering Rules

- Keep domain free of application, persistence, and API dependencies.
- Put business invariants on domain entities/value objects.
- Use MediatR handlers in Application for use cases.
- Return `FluentResults` from handlers and map failures through `ResultExtensions`.
- Use EF repositories plus `IUnitOfWork` for aggregate writes.
- Use query abstractions and Dapper read models for read-side DTO queries where that pattern already exists.
- Put EF mappings in `ProductsAndPricingNew.Persistence/Configuration`.

## Common Commands

- Restore: `dotnet restore ProductsAndPricingNew.sln --locked-mode`
- Format check: `dotnet format ProductsAndPricingNew.sln --verify-no-changes --severity error --no-restore`
- Build: `dotnet build ProductsAndPricingNew.sln --configuration Release --no-restore`
- Test all: `dotnet test ProductsAndPricingNew.sln --configuration Release --no-restore`

## Coding Notes

- Nullable reference types, implicit usings, analyzers, locked restore, and warnings-as-errors are enabled in `Directory.Build.props`.
- Follow `.editorconfig`: prefer explicit types over `var`, allow omitted braces, and keep formatting clean.
- Add or update focused tests for meaningful behavior changes.
- Do not add migrations unless the task explicitly asks; no migrations are currently present in this repo.
