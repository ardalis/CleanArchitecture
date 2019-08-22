using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IDatabaseRegistrar
    {
        void RegisterInMemory(IServiceCollection services, string dbName = null);
        void RegisterSQLite(IServiceCollection services);
        void RegisterSQLServer(IServiceCollection services, string connectionString);
    }
}
