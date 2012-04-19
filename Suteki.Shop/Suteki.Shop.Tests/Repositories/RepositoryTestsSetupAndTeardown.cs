using System;
using NUnit.Framework;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Repositories;
using Suteki.Shop.Tests.Maps;

namespace Suteki.Shop.Tests.Repositories
{
    [SetUpFixture]
    public class RepositoryTestsSetupAndTeardown
    {
        [SetUp]
        public void RunBeforeAnyMapTests()
        {
            InMemoryDatabaseManager.SetMappingConfiguration(new SutekiShopMappingConfiguration());
            InMemoryDatabaseManager.Start();
            Console.WriteLine("Opening in memory database connection");
        }

        [TearDown]
        public void RunAfterAnyMapTests()
        {
            InMemoryDatabaseManager.Stop();
            Console.WriteLine("Closing in memory database connection");
        }
    }
}