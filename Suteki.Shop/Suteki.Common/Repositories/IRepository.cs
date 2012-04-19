using System.Linq;

namespace Suteki.Common.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        void SaveOrUpdate(T entity);
        void DeleteOnSubmit(T entity);
    }

    public interface IRepository
    {
        object GetById(int id);
        IQueryable GetAll();
        void SaveOrUpdate(object entity);
        void DeleteOnSubmit(object entity);
    }

    public interface IRepositoryFactory<T>
        where T : class
    {
        IRepository<T> Resolve();
        void Release(IRepository<T> repository);
    }
}