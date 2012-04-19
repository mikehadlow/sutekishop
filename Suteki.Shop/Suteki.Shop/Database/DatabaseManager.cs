using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Suteki.Shop.Database
{
    public class DatabaseManager
    {
        private readonly Configuration configuration;
        private const string createDatabaseSql = "create database {0}";
        private const string dropDatabaseSql = "drop database {0}";
        private const string checkExistsSql = "select * from sysdatabases where name = '{0}'";
        private const string connectionStringTemplate = "Data Source={0};Initial Catalog=master;{1}";
        private const string connectionStringKey = "connection.connection_string";
        private string connectionString;

        public DatabaseManager(Configuration configuration)
        {
            this.configuration = configuration;
            
        }

        public void CreateDatabase()
        {
            RunCommand(string.Format(createDatabaseSql, GetDbName()), command => command.ExecuteNonQuery());

            new SchemaExport(configuration).Create(false, true);
        }

        public bool DatabaseExists
        {
            get
            {
                var dbExists = false;
                RunCommand(string.Format(checkExistsSql, GetDbName()), command =>
                    {
                        using (var reader = command.ExecuteReader())
                            dbExists = reader.Read();
                    });
                return dbExists;
            }
        }

        public void DropDatabase()
        {
            RunCommand(string.Format(dropDatabaseSql, GetDbName()), command => command.ExecuteNonQuery());
        }

        private void RunCommand(string sql, Action<IDbCommand> commandAction)
        {
            using (var connection = new SqlConnection(GetMasterConnectionString()))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                commandAction(command);
            }
        }

        public string GetConnectionString()
        {
            if (connectionString != null) return connectionString;

            if (configuration.Properties.ContainsKey(connectionStringKey))
            {
                connectionString = configuration.Properties[connectionStringKey];
                Console.Out.WriteLine("connectionString = '{0}'", connectionString);
                return connectionString;
            }
            throw new DatabaseCreationException(string.Format("NHibernate configuration, property: '{0}' is not set.",
                                                              connectionStringKey));
        }

        public string GetDbName()
        {
            return DatabaseNameFromConnectionString(GetConnectionString());
        }

        public static string DatabaseNameFromConnectionString(string connectionString)
        {
            return ValueFromKey(connectionString, "Initial Catalog");
        }

        public string GetServerName()
        {
            return ServerNameFromConnectionString(GetConnectionString());
        }

        public static string ServerNameFromConnectionString(string connectionString)
        {
            return ValueFromKey(connectionString, "Data Source");
        }

        private static string ValueFromKey(string connectionString, string key)
        {
            var match = Regex.Match(connectionString, string.Format(@"{0}=([\w\\\(\)_\-\.]+);", key));
            if (!match.Success)
            {
                throw new DatabaseCreationException(string.Format("Could not find '{0}' in connection string", key));
            }
            return match.Groups[1].Value;
        }

        public string GetMasterConnectionString()
        {
            const string security = "Integrated Security=SSPI;";

            return string.Format(connectionStringTemplate, GetServerName(), security);
        }
    }

    [Serializable]
    public class DatabaseCreationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DatabaseCreationException()
        {
        }

        public DatabaseCreationException(string message) : base(message)
        {
        }

        public DatabaseCreationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DatabaseCreationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}