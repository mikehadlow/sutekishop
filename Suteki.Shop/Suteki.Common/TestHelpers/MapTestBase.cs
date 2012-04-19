using System;
using System.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Suteki.Common.Repositories;

namespace Suteki.Common.TestHelpers
{
    public static class InMemoryDatabaseManager
    {
        private static Configuration configuration;
        private static IDbConnection connection;
        private static ISessionFactory sessionFactory;
        private static IMappingConfigurationContributor configurationContributor;
        static readonly object sessionFactoryLock = new object();

        /// <summary>
        /// Fixture setup and teardown doesn't seem to work in some build environments, so 
        /// fall back to lazy initialisation.
        /// </summary>
        private static ISessionFactory SessionFactory
        {
            get
            {
                if(sessionFactory == null)
                {
                    lock (sessionFactoryLock)
                    {
                        if (sessionFactory == null)
                        {
                            Start();
                        }
                    }
                }
                return sessionFactory;
            }
        }

        public static void Start()
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            sessionFactory = GetSessionFactory();
            connection = OpenConnection();
            BuildSchema(OpenSession());
        }

        public static void Stop()
        {
            sessionFactory.Dispose();
            connection.Dispose();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession(connection);
        }

        private static ISessionFactory GetSessionFactory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                .Mappings(mappingConfiguration => FluentNHibernateConfigurationBuilder.ConfigureMappings(mappingConfiguration, configurationContributor))
                .ExposeConfiguration(c => configuration = c)
                .BuildSessionFactory();
        }

        private static void BuildSchema(ISession session)
        {
            var export = new SchemaExport(configuration);
            export.Execute(true, true, false, session.Connection, null);
        }

        private static IDbConnection OpenConnection()
        {
            var sqLiteConnection = new System.Data.SQLite.SQLiteConnection("Data Source=:memory:;Version=3;New=True");
            sqLiteConnection.Open();
            return sqLiteConnection;
        }

        public static void SetMappingConfiguration(IMappingConfigurationContributor contributor)
        {
            configurationContributor = contributor;
        }
    }

    public abstract class MapTestBase
    {
        protected void SetMappingConfiguration(IMappingConfigurationContributor configurationContributor)
        {
            InMemoryDatabaseManager.SetMappingConfiguration(configurationContributor);
        }

        protected ISession OpenSession()
        {
            return InMemoryDatabaseManager.OpenSession();
        }

        protected void InSession(Action<ISession> action)
        {
            using (var session = InMemoryDatabaseManager.OpenSession())
            using(var transaction = session.BeginTransaction())
            {
                action(session);
                transaction.Commit();
            }
        }
    }
}