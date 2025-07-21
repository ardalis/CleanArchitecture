# ${name}

Welcome to your new project generated with the Clean Architecture template!

## Getting Started

- Build the solution:
  ```sh
  dotnet build
  ```
- Run the application:
  ```sh
  dotnet run --project src/Your.ProjectName.Web
  ```
- Update the database (if using EF Core):
  ```sh
  dotnet ef database update -c AppDbContext -p src/Your.ProjectName.Infrastructure/Your.ProjectName.Infrastructure.csproj -s src/Your.ProjectName.Web/Your.ProjectName.Web.csproj
  ```

## Solution Structure

- **Core**: Domain entities, value objects, interfaces
- **UseCases**: Application logic, CQRS handlers
- **Infrastructure**: Data access, external services
- **Web**: API endpoints (FastEndpoints)

For more details, see the documentation or visit the [Clean Architecture Template repository](https://github.com/ardalis/CleanArchitecture).
