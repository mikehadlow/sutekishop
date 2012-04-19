using System;
using System.Reflection;
using NUnit.Framework;
using Suteki.Common.Models;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Tests.Models.Builders;

namespace Suteki.Shop.Tests.Maps
{
    /// <summary>
    /// Runs a simple test for each entity listed. That it can be saved to the database and retrieved.
    /// </summary>
    [TestFixture]
    public class SimpleMapTests : MapTestBase
    {
        [Test, TestCaseSource(typeof(ModelTestData), "AllEntityTypes")]
        public void SimpleMapTest(Type entityType)
        {
            var runSimpleMapTest = GetType().GetMethod("RunSimpleMapTest");
            var runSimpleMapTestForType = runSimpleMapTest.MakeGenericMethod(entityType);
            runSimpleMapTestForType.Invoke(this, new object[0]);
        }

        public void RunSimpleMapTest<TEntity>() where TEntity : IEntity
        {
            var id = 0;

            InSession(session =>
            {
                var entity = GetDefaultsFor<TEntity>();

                session.Save(entity);
                id = entity.Id;
            });

            InSession(session => session.Get<TEntity>(id));
        }

        static TEntity GetDefaultsFor<TEntity>()
        {
            var entityName = typeof (TEntity).Name;
            var defaults = typeof(Default).GetMethod(entityName, BindingFlags.Static | BindingFlags.Public);
            if (defaults == null)
            {
                Assert.Fail(string.Format("No static method Default.{0} found", entityName));
            }
            if (defaults.ReturnType != typeof (TEntity))
            {
                Assert.Fail(string.Format("Method Default.{0} does not have a return type of {0}", entityName));
            }

            return (TEntity) defaults.Invoke(null, new object[0]);
        }
    }
}