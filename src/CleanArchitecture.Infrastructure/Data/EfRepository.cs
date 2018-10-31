using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Infrastructure.Data
{
    public class EfRepository : IRepository
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById<T>(int id) where T : BaseEntity
        {
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public List<T> List<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>().ToList();
        }

        public T Add<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }
        public List<T> Add<T>(List<T> entities) where T : BaseEntity
        {
            _dbContext.Set<T>().AddRange(entities);
            _dbContext.SaveChanges();

            return entities;
        }
        public void Delete<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }
        public void Delete<T>(List<T> entities) where T : BaseEntity
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }
        public void Update<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public void Update<T>(List<T> entities) where T : BaseEntity
        {
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }
    }
}
