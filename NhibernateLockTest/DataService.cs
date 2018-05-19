using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace NhibernateLockTest
{
    public class DataService
    {
        private static string  SqLiteConnectionString = "Data Source=test.db;Version=3;Journal Mode=Off;Connection Timeout=120";

        private static string MsqlConnectionString = "Integrated Security=SSPI;Persist Security Info=False;Data Source=localhost;Initial Catalog=NHibernateTest;MultipleActiveResultSets=true;Connection Timeout=120";

        private static ISessionFactory Factory { get; set; }
        private static DbConnection Connection { get; set; }

        static DataService()
        {
            Factory = CreateSessionFactory();
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var configuration = Fluently.Configure()
                    //.Database(SQLiteConfiguration.Standard.ConnectionString(SqLiteConnectionString).IsolationLevel(IsolationLevel.ReadCommitted))
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(MsqlConnectionString).IsolationLevel(IsolationLevel.ReadCommitted))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TestClass>())
                    .BuildConfiguration();

            var result =  configuration.BuildSessionFactory();

            new SchemaUpdate(configuration).Execute(false, true);

            return result;
        }

        public static ISession OpenSession()
        {
            if (Connection == null)
            {
                CreateConnection();
            }

            return Factory.WithOptions().Connection(Connection).OpenSession();
        }

        public static IStatelessSession OpenStatelessSession()
        {
            if (Connection == null)
            {
                CreateConnection();
            }

            return Factory.OpenStatelessSession(Connection);
        }

        private static void CreateConnection()
        {
            //Connection = new SQLiteConnection(SqLiteConnectionString);
            Connection = new System.Data.SqlClient.SqlConnection(MsqlConnectionString);
            Connection.Open();
        }
    }
}