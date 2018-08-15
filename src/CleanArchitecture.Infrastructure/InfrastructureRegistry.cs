using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure
{
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For(typeof(IRepository<>)).Add(typeof(EfRepository<>));
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());            

            For<AppDbContext>().Add<AppDbContext>().Ctor<DbContextOptions<AppDbContext>>().Is(
                (DbContextOptions<AppDbContext>)optionsBuilder.Options);
            //    //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
