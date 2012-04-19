using System;
using System.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.SqlTypes;
using NHibernate.Tool.hbm2ddl;
using Suteki.Common.NHibernate;

namespace Suteki.Shop.Tests.Spikes
{
    public class UserTypeSpike
    {
        public void Should_be_able_to_user_custom_userType_with_NHibernate()
        {
            var configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                .Mappings(x => x.FluentMappings
                    .Add(typeof (WidgetMap)))
                .BuildConfiguration();

            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                BuildSchema(session, configuration);
                var id = 0;
                using (var transaction = session.BeginTransaction())
                {
                    var widget = new Widget {Name = new My{ Value = "Das Widget"}};
                    session.Save(widget);
                    transaction.Commit();
                    id = widget.Id;
                    Console.Out.WriteLine("id = {0}", id);
                    session.Evict(widget);
                }

                using (var transaction = session.BeginTransaction())
                {
                    var widget = session.Load<Widget>(id);
                    Console.Out.WriteLine("widget.Name.Value = {0}", widget.Name.Value);
                    transaction.Commit();
                }
            }
        }

        private static void BuildSchema(ISession session, Configuration configuration)
        {
            var export = new SchemaExport(configuration);
            export.Execute(true, true, false, session.Connection, null);
        }
    }

    public class Widget
    {
        public virtual int Id { get; set; }
        public virtual My Name { get; set; }
    }

    public class WidgetMap : ClassMap<Widget>
    {
        public WidgetMap()
        {
            Id(x => x.Id);
            // convention doesn't work, have to explicitly ask to use user type...
            Map(x => x.Name).CustomType<MyUserType>(); 
        }
    }

    public class My
    {
        public string Value { get; set; }
    }

    public class MyConvention : UserTypeConvention<MyUserType>{}

    public class MyUserType : BaseImmutableUserType<MyUserType>
    {
        public override object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var value = NHibernateUtil.String.NullSafeGet(rs, names[0]) as string;
            return new My {Value = value};
        }

        public override void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var my = value as My;
            NHibernateUtil.String.NullSafeSet(cmd, my, index);
        }

        public override SqlType[] SqlTypes
        {
            get { return new SqlType[]{ SqlTypeFactory.GetString(20) }; }
        }
    } 
}