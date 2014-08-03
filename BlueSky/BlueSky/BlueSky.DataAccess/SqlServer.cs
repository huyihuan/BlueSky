using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.SqlClient;
using BlueSky.Interfaces;

namespace BlueSky.DataAccess
{
    public class SqlServer : DbBase
    {
        public override DatabaseType DbType
        {
            get
            {
                return DatabaseType.SqlServer;
            }
        }
        public override DbProviderFactory DbFactory
        {
            get
            {
                return SqlClientFactory.Instance;
            }
        }
    }
}
