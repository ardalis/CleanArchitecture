using Ardalis.GuardClauses;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CleanArchitecture.Infrastructure.Data
{
    public class DatabaseRegistrar : IDatabaseRegistrar
    {
        public void RegisterInMemory(IServiceCollection services, string dbName)
        {
            string databaseName = dbName ?? Guid.NewGuid().ToString();
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName));
        }

        public void RegisterSQLite(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=database.sqlite")); // will be created in web project root
        }

        public void RegisterSQLServer(IServiceCollection services, string connectionString)
        {
            Guard.Against.NullOrWhiteSpace(connectionString, nameof(connectionString));

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
