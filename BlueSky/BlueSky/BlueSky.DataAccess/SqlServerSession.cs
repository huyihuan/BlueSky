using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Interfaces;

namespace BlueSky.DataAccess
{
    public class SqlServerSession : DbSession, IDbSession
    {
        static SqlServerSession()
        {
            DbSessionFactory.Register(DatabaseType.SqlServer, typeof(SqlServerSession));
        }
        public override IDatabase OnCreateDatabase()
        {
            return new SqlServer();
        }
    }
}
