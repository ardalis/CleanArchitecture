using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure
{
	public static class StartupSetup
	{
		public static void AddDbContext(this IServiceCollection services) =>
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlite("Data Source=database.sqlite")); // will be created in web project root
	}
}
