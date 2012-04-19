using System;
using System.Linq;
using Suteki.Common.Repositories;

namespace Suteki.Shop.StockControl.AddIn.Tests
{
    public class DummyRepository<T> : IRepository<T> where T : class
    {
        public Func<int, T> GetByIdDelegate { get; set; }
        public Func<IQueryable<T>> GetAllDelegate { get; set; }
        public Action<T> SaveOrUpdateDelegate { get; set; }
        public Action<T> DeleteOnSubmitDelegate { get; set; }

        public DummyRepository()
        {
            GetByIdDelegate = id => null;
            GetAllDelegate = () => null;
            SaveOrUpdateDelegate = x => { };
            DeleteOnSubmitDelegate = x => { };
        }

        public T GetById(int id)
        {
            return GetByIdDelegate(id);
        }

        public IQueryable<T> GetAll()
        {
            return GetAllDelegate();
        }

        public void SaveOrUpdate(T entity)
        {
            SaveOrUpdateDelegate(entity);
        }

        public void DeleteOnSubmit(T entity)
        {
            DeleteOnSubmitDelegate(entity);
        }
    }
}