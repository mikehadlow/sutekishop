using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Suteki.Common.Extensions;
using Suteki.Common.Models;

namespace Suteki.Common.Repositories
{
    public static class QueriableExtensions
    {
        const string expressionNotProperty = "propertyExpression must be a property accessor. e.g: 'x => x.MyProperty'";

        public static IQueryable<T> WithSelectItem<T>(this IQueryable<T> items, Expression<Func<T, string>> propertyExpression) 
            where T : new()
        {
            if (propertyExpression.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException(expressionNotProperty);
            }
            
            var memberExpression = propertyExpression.Body as MemberExpression;
            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(expressionNotProperty);
            }

            var selectItem = new T();
            property.SetValue(selectItem, "<select>", null);

            var selectItems = new List<T> { selectItem };
            return selectItems.Union(items.ToList()).AsQueryable();
        }

        public static IQueryable<T> NotIncluding<T>(this IQueryable<T> items, int primaryKey)
            where T : IEntity
        {
            return items.Where(x => x.Id != primaryKey);
            //            var itemParameter = Expression.Parameter(typeof(T), "item");
            //
            //            var whereExpression = Expression.Lambda<Func<T, bool>>
            //                (
            //                Expression.NotEqual(
            //                    Expression.Property(
            //                        itemParameter,
            //                        typeof(T).GetPrimaryKey().Name
            //                        ),
            //                    Expression.Constant(primaryKey)
            //                    ),
            //                new[] { itemParameter }
            //                );
            //            return items.Where(whereExpression);
        }
    }
}
