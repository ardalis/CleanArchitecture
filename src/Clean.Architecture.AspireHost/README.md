# Clean Architecture Aspire Host

This project uses .NET Aspire to orchestrate the application and its dependencies.

## SQL Server Container

The Aspire host is configured to run a SQL Server container and automatically provides the connection string to the Web application.

### Running the Application

1. Set `Clean.Architecture.AspireHost` as the startup project
2. Run the application (F5 or Ctrl+F5)
3. The Aspire Dashboard will open, showing all running resources including the SQL Server container
4. The Web application will automatically connect to the SQL Server container

### Connection String

When running through Aspire, the connection string is automatically provided by Aspire and will override the `DefaultConnection` in appsettings.json. The connection is named "cleanarchitecture" and is referenced in the Web project.

### Running Without Aspire

If you run the Web project directly (not through AspireHost), it will fall back to using the SQLite connection string from appsettings.json.

### Database Migrations

The existing migrations were created for SQLite but will work with SQL Server as well. If you need to create a new migration:

From the Web project directory:
```bash
dotnet ef migrations add MigrationName -c AppDbContext -p ../Clean.Architecture.Infrastructure/Clean.Architecture.Infrastructure.csproj -s Clean.Architecture.Web.csproj -o Data/Migrations
```

To update the database:
```bash
dotnet ef database update -c AppDbContext -p ../Clean.Architecture.Infrastructure/Clean.Architecture.Infrastructure.csproj -s Clean.Architecture.Web.csproj
```

Note: When running through Aspire, the database will be automatically created in the SQL Server container if it doesn't exist.

### Container Persistence

The SQL Server container is configured with `ContainerLifetime.Persistent`, which means the data will persist between application runs. To reset the database, you can:
1. Delete the container through the Aspire dashboard
2. Use the Docker CLI: `docker rm <container-name>`
