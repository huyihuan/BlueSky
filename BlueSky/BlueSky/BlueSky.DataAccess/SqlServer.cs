using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.SqlClient;
using BlueSky.Interfaces;
using BlueSky.Extensions;
using System.Configuration;

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
        protected override string DefaultConncetionName
        {
            get 
            {
                return "SqlServerConnectionString";
            }
        }
        public override string ConnectionString
        {
            get
            {
                if (base.ConnectionString.IsNullOrEmpty())
                {
                    if (this.ConnectionName.IsNullOrEmpty())
                        this.ConnectionName = this.DefaultConncetionName;
                    base.ConnectionString = ConfigurationManager.AppSettings[this.ConnectionName];
                }
                return base.ConnectionString;
            }
            set
            {
                base.ConnectionString = value;
            }
        }
    }
}
