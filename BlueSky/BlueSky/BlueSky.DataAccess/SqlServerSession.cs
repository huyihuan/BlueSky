using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueSky.DataAccess
{
    public class SqlServerSession : DbSession
    {
        public SqlServerSession()
        {
            base.Database = new SqlServer();
        }
    }
}
