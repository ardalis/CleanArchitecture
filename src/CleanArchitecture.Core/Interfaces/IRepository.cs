using CleanArchitecture.Core.SharedKernel;
using System.Collections.Generic;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IRepository
    {
        T GetById<T>(int id) where T : BaseEntity;
        List<T> List<T>() where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        List<T> Add<T>(List<T> entities) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Update<T>(List<T> entities) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
        void Delete<T>(List<T> entities) where T : BaseEntity;
    }
}
