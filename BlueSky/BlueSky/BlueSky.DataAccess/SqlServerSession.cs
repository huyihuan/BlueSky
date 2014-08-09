using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Interfaces;

namespace BlueSky.DataAccess
{
    public class SqlServerSession : DbSession
    {
        public override IDatabase OnCreateDatabase()
        {
            return new SqlServer();
        }
    }
}
