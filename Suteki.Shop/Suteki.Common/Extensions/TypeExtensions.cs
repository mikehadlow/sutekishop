using System;
using System.Collections;
using System.Reflection;
using Suteki.Common.Models;

namespace Suteki.Common.Extensions
{
    public static class TypeExtensions
    {
        public static PropertyInfo GetPrimaryKey(this Type entityType)
        {
            if(!entityType.IsEntity())
            {
                throw new ApplicationException(string.Format("type {0} does not implement IEntity", entityType.Name));
            }
            return entityType.GetProperty("Id");
        }

        public static bool IsEntity(this Type type)
        {
            return typeof(IEntity).IsAssignableFrom(type);
        }

        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static bool IsOrderable(this Type type)
        {
            return typeof (IOrderable).IsAssignableFrom(type);
        }

        public static bool IsActivatable(this Type type)
        {
            return typeof (IActivatable).IsAssignableFrom(type);
        }
    }
}