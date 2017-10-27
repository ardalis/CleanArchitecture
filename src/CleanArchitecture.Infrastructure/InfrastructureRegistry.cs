using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using StructureMap;

namespace CleanArchitecture.Infrastructure
{
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For(typeof(IRepository<>)).Add(typeof(EfRepository<>));
        }
    }
}
