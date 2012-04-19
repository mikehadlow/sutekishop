using System;

namespace Suteki.Common.Repositories
{
    public interface IRepositoryResolver
    {
        IRepository<T> GetRepository<T>() where T : class;
        IRepository GetRepository(Type type);
        void Release(object instance);
    }
}