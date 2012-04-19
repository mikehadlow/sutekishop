using System;
using System.Linq;
using Suteki.Common.Models;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Tests
{
    public class DummyRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public Func<int, TEntity> GetByIdDelegate { get; set; }
        public Func<IQueryable<TEntity>> GetAllDelegate { get; set; }
        public Action<TEntity> SaveOrUpdateDelegate { get; set; }
        public Action<TEntity> DeleteOnSubmitDelegate { get; set; }

        public TEntity GetById(int id)
        {
            return GetByIdDelegate(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return GetAllDelegate();
        }

        public void SaveOrUpdate(TEntity entity)
        {
            SaveOrUpdateDelegate(entity);
        }

        public void DeleteOnSubmit(TEntity entity)
        {
            DeleteOnSubmitDelegate(entity);
        }
    }
}