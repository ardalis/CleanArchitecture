using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Data
{
    public class DatabaseRegistrar : IDatabaseRegistrar
    {
        public void Register(IServiceCollection services)
        {
            // string dbName = Guid.NewGuid().ToString();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=database.sqlite")); // will be created in web project root
            //    options.UseInMemoryDatabase(dbName));
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
