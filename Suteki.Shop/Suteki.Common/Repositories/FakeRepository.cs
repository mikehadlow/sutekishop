using System;
using System.Collections.Generic;
using System.Linq;

namespace Suteki.Common.Repositories
{
    public class FakeRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        public Func<int, TEntity> EntityFactory { get; set; }
        public IList<TEntity> EntitesToReturnFromGetAll { get; private set; }
        public IList<TEntity> EntitiesSubmitted { get; private set; }
        public IList<TEntity> EntitiesDeleted { get; private set; }
        public bool SubmitChangesWasCalled { get; private set; }
        public int EntitiesReturnedFromGetById { get; private set; }

        public Action<TEntity> SaveOrUpdateDelegate { get; set; }

        public FakeRepository() : this(id => null)
        {
        }

        public FakeRepository(Func<int, TEntity> entityFactory)
        {
            EntitesToReturnFromGetAll = new List<TEntity>();
            EntitiesSubmitted = new List<TEntity>();
            EntitiesDeleted = new List<TEntity>();
            SubmitChangesWasCalled = false;
            EntitiesReturnedFromGetById = 0;
            EntityFactory = entityFactory;
            SaveOrUpdateDelegate = entity => EntitiesSubmitted.Add(entity);
        }

        public TEntity GetById(int id)
        {
            EntitiesReturnedFromGetById++;
            return EntityFactory(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return EntitesToReturnFromGetAll.AsQueryable();
        }

        public void SaveOrUpdate(TEntity entity)
        {
            SaveOrUpdateDelegate(entity);
        }

        public void DeleteOnSubmit(TEntity entity)
        {
            EntitiesDeleted.Add(entity);
        }

        public void SubmitChanges()
        {
            SubmitChangesWasCalled = true;
        }
    }

    public class FakeRepository : FakeRepository<object>, IRepository
    {
        public FakeRepository(Func<int, object> entityFactory) : base(entityFactory)
        {
        }

        public new IQueryable GetAll()
        {
            return EntitesToReturnFromGetAll.AsQueryable();
        }
    }
}