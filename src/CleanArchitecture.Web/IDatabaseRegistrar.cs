using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Web
{
    public interface IDatabaseRegistrar
    {
        void RegisterDb(IServiceCollection services, string connectionString);
    }
}
