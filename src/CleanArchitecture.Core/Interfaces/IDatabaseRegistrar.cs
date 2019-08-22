using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IDatabaseRegistrar
    {
        void Register(IServiceCollection services);
    }
}
