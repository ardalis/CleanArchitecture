using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public List<T> List()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T Add(T entity)
        {
            // if using in memory EF, need to support IDENTITY keys
            //if (entity.Id == 0)
            //{
            //    int newId = 1;
            //    var entities = List();
            //    if (entities.Any())
            //    {
            //        newId = entities.Max(z => z.Id) + 1;
            //    }
            //    entity.Id = newId;
            //}
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}