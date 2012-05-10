using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Suteki.Common.Utils
{
    public class ExpressionHelper
    {
        const string expressionNotProperty = "propertyExpression must be a property accessor. e.g: 'x => x.MyProperty'";

        public static string GetDottedPropertyNameFromExpression<TModel, TProperty>(Expression<Func<TModel, TProperty>> propertyExpression)
        {
            return string.Join(".", GetProperties(propertyExpression)
                .Select(property => property.Name));
        }

        public static IEnumerable<PropertyInfo> GetProperties<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return GetPropertiesInternal(propertyExpression.Body);
        }

        private static IEnumerable<PropertyInfo> GetPropertiesInternal(Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null) yield break;

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new SutekiCommonException(expressionNotProperty);
            }
            foreach (var propertyInfo in GetPropertiesInternal(memberExpression.Expression))
            {
                yield return propertyInfo;
            }
            yield return property;
        }
    }
}