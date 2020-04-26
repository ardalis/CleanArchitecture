using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.SharedKernel.Interfaces
{
    public interface IRepository
    {
        Task<T> GetByIdAsync<T>(int id) where T : BaseEntity;
        List<T> List<T>() where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
    }
}