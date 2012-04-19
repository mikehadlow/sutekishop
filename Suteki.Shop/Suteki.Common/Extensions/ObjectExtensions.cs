using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Proxy;

namespace Suteki.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns all the public properties as a list of Name, Value pairs
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<NameValue<object>> GetProperties(this object item)
        {
            foreach (PropertyInfo property in item.GetType().GetProperties())
            {
                yield return new NameValue<object>(property.Name, () => property.GetValue(item, null));
            }
        }

        public static void WriteProperties(this object item)
        {
            item.WriteProperty(-1);
        }

        public static void WriteProperty(this object item, int level)
        {
            level++;
            if(item == null)
            {
                Console.WriteLine();
                return;
            }

            var items = item as IEnumerable;
            if (items != null && item.GetType() != typeof(string))
            {
                Console.WriteLine();
                foreach (var child in items)
                {
                    Console.Write("{0}", new string('\t', level));
                    child.WriteProperty(level);
                }
                return;
            }

            Console.WriteLine("{0}", item);
        }

		public static string ToYesNo(this bool source)
		{
			return source.ToString().Replace(bool.TrueString, "Yes").Replace(bool.FalseString, "No");
		}

        public static T CastAs<T>(this object source) where T : class
        {
            if (source is INHibernateProxy)
            {
                var type = NHibernateUtil.GetClass(source);
                if (type != typeof(T))
                {
                    throw new ApplicationException(string.Format("Cannot cast {0} to {1}", type.Name, typeof(T).Name));
                }

                return ((INHibernateProxy)source).HibernateLazyInitializer.GetImplementation() as T;
            }
            return source as T;
        }

        public static IEnumerable<T> Single<T>(this T item)
        {
            yield return item;
        }
    }
}