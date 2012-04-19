using System;
using NUnit.Framework;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Tests.Maps
{
    [SetUpFixture]
    public class MapsTestsSetupAndTeardown
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