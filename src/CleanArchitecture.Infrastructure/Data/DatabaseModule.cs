using Autofac;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace CleanArchitecture.Infrastructure.Data
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string dbName = Guid.NewGuid().ToString();
            var option = new DbContextOptionsBuilder<AppDbContext>();
            var dbContextOptions = option.UseInMemoryDatabase(dbName).Options;
            // var dbContextOptions = option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            builder.Register(t => new AppDbContext(dbContextOptions, t.Resolve<IDomainEventDispatcher>()));
        }
    }
}
