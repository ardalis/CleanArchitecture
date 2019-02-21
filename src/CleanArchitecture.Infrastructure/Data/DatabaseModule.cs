using Autofac;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace CleanArchitecture.Infrastructure.Data
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                DbContextOptions<AppDbContext> dbContextOptions = GetInMemoryDbContextOptions();
                // DbContextOptions<AppDbContext> dbContextOptions = GetSqlServerDbContextOptions(context);
                return new AppDbContext(dbContextOptions, context.Resolve<IDomainEventDispatcher>());
            }).SingleInstance();
        }

        private static DbContextOptions<AppDbContext> GetInMemoryDbContextOptions()
        {
            string dbName = Guid.NewGuid().ToString();
            var option = new DbContextOptionsBuilder<AppDbContext>();
            return option.UseInMemoryDatabase(dbName).Options;
        }

        private static DbContextOptions<AppDbContext> GetSqlServerDbContextOptions(IComponentContext context)
        {
            IConfiguration config = context.Resolve<IConfiguration>();
            var option = new DbContextOptionsBuilder<AppDbContext>();
            return option.UseSqlServer(config.GetConnectionString("DefaultConnection")).Options;
        }
    }
}
