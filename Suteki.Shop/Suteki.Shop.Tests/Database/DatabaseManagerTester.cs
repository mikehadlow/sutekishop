using System;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NUnit.Framework;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;
using Suteki.Shop.Database;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Tests.Database
{
    [TestFixture]
    public class DatabaseManagerTester
    {
        private DatabaseManager databaseManager;
        private Configuration configuration;

        [SetUp]
        public void SetUp()
        {
            var configurationBuilder = new FluentNHibernateConfigurationBuilder(new[]{ new SutekiShopMappingConfiguration() });
            configuration = configurationBuilder.BuildConfiguration(
                MsSqlConfiguration.MsSql2005.ConnectionString(
                    c => c.FromConnectionStringWithKey("SutekiShopConnectionString")));

            databaseManager = new DatabaseManager(configuration);
        }

        [Test, Explicit("Only run this test if you want to create the database!")]
        public void CreateDatabase()
        {
            if (databaseManager.DatabaseExists)
            {
                Console.WriteLine("Dropping Database {0}", databaseManager.GetDbName());
                databaseManager.DropDatabase();
            }
            Console.WriteLine("Creating Database {0}", databaseManager.GetDbName());
            databaseManager.CreateDatabase();

            Console.WriteLine("Inserting Static Data");
            new StaticDataGenerator(configuration).Insert();

            Console.WriteLine("Successfully Created Database {0}", databaseManager.GetDbName());
        }

        [Test]
        public void ParseConnectionString()
        {
            const string connectionString = "Data Source=SomeServerName;Initial Catalog=SomeDbName;Integrated Security=True";

            DatabaseManager.DatabaseNameFromConnectionString(connectionString).ShouldEqual("SomeDbName");
            DatabaseManager.ServerNameFromConnectionString(connectionString).ShouldEqual("SomeServerName");
        }

        [Test]
        public void ParseConnectionString_should_handle_server_names_with_non_numeric_characters()
        {
            const string connectionStringTemplate = "Data Source={0};Initial Catalog=SomeDbName;Integrated Security=True";

            Action<string> canGetServerName = serverName =>
            {
                var connectionString = connectionStringTemplate.With(serverName);
                DatabaseManager.ServerNameFromConnectionString(connectionString).ShouldEqual(serverName);
            };

            canGetServerName(@"MyServer\MyInstance");
            canGetServerName(@"My-New-Server");
            canGetServerName(@"My_New_Server");
            canGetServerName(@".");
            canGetServerName(@".\instance");
        }

        [Test]
        public void GetMasterConnectionStringTest()
        {
            databaseManager.GetMasterConnectionString().ShouldEqual("Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;");
        }
    }
}