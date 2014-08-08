using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.SqlClient;
using BlueSky.Interfaces;
using BlueSky.Extensions;

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
        private string _ConnectionString;
        public override string ConnectionString
        {
            get
            {
                if (this._ConnectionString.IsNullOrEmpty())
                {
                    this._ConnectionString = System.Configuration.ConfigurationManager.AppSettings[base.ConnectionName = "ConnectionString"];
                }
                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }
    }
}
