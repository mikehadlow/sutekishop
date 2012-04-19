using System;
using System.Collections.Generic;

namespace Suteki.Shop.Tests.Models.Builders
{
    public class TestDataFor<TEntity>
        where TEntity : class, new() 
    {
        readonly Func<TEntity> defaultBuilder;
        readonly IList<Action<TEntity>> propertySetters = new List<Action<TEntity>>();

        public TestDataFor(Func<TEntity> defaultBuilder)
        {
            this.defaultBuilder = defaultBuilder;
        }

        TEntity Build()
        {
            var entity = defaultBuilder();

            foreach (var propertySetter in propertySetters)
            {
                propertySetter(entity);
            }

            return entity;
        }

        public static implicit operator TEntity(TestDataFor<TEntity> builder)
        {
            return builder.Build();
        }

        /// <summary>
        /// Allow default properties to be changed before building
        /// </summary>
        /// <param name="propertySetter"></param>
        public TestDataFor<TEntity> With(Action<TEntity> propertySetter)
        {
            propertySetters.Add(propertySetter);
            return this;
        }
    }

    public static class TestData
    {
        public static TestDataFor<TEntity> For<TEntity>(Func<TEntity> defaultBuilder) where TEntity : class, new()
        {
            return new TestDataFor<TEntity>(defaultBuilder);
        }
    }
}