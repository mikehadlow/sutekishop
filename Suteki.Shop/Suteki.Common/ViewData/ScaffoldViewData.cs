using System;
using System.Collections;
using System.Collections.Generic;
using Suteki.Common.Extensions;

namespace Suteki.Common.ViewData
{
    public class ScaffoldViewData<T> : ViewDataBase
    {
        public IEnumerable<T> Items { get; set; }
        public T Item { get; set; }

        private readonly Dictionary<Type, object> lookupLists = new Dictionary<Type, object>();

        public ScaffoldViewData<T> With(T item)
        {
            Item = item;
            return this;
        }

        public ScaffoldViewData<T> With(IEnumerable<T> items)
        {
            Items = items;
            return this;
        }

        public ScaffoldViewData<T> WithLookupList(Type entityType, object items)
        {
            lookupLists.Add(entityType, items);
            return this;
        }

		public IEnumerable<TLookup> GetLookupList<TLookup>()
		{
			return (IEnumerable<TLookup>) GetLookupList(typeof(TLookup));
		}

        public IEnumerable GetLookupList(Type lookupType)
        {
            if (!lookupLists.ContainsKey(lookupType))
            {
                throw new ApplicationException("List of type {0} does not exist in lookup list".With(
                    lookupType.Name));
            }
            return (IEnumerable)lookupLists[lookupType];
        }

        public string EntityName
        {
            get
            {
                return typeof(T).Name;                
            }
        }
    }

    public static class ScaffoldView
    {
        public static ScaffoldViewData<T> Data<T>()
        {
            return new ScaffoldViewData<T>();
        }
    }
}
