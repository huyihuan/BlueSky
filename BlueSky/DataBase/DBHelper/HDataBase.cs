using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;
using System.Configuration;

namespace DataBase.DBHelper
{
    public class HDataBase : System.IDisposable
    {
        private string _ConnectionString = string.Empty;
        private HDBType _DatabaseType = HDBType.SqlServer;
        private IDbConnection _Connection = null;

        public HDataBase()
        {
            _ConnectionString = GetConnectionString();
            string strType = GetDbType();
            switch (strType)
            { 
                case "sqlserver":
                    _DatabaseType = HDBType.SqlServer;
                    break;
                case "mysql":
                    _DatabaseType = HDBType.MySql;
                    break;
                case "oracle":
                    _DatabaseType = HDBType.Oracle;
                    break;
            }
            CreateDBConnection();
        }
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { this._ConnectionString = value; }
        }
        public HDBType DataBaseType
        {
            get { return _DatabaseType; }
            set { this._DatabaseType = value; }
        }
        public IDbConnection Connection
        {
            get { return _Connection; }
            set { _Connection = value; }
        }

        private static string GetConnectionString()
        {
            string strConStr = ConfigurationManager.AppSettings["ConnectionString"] + "";
            return strConStr;
        }

        private static string GetDbType()
        {
            string type = ConfigurationManager.AppSettings["DbType"] + "";
            if (null == type || "" == type)
                type = "sqlserver";
            return type;
        }

        private void CreateDBConnection()
        {
            switch (_DatabaseType)
            { 
                case HDBType.SqlServer:
                    Connection = new SqlConnection();
                    break;
                case HDBType.MySql:
                    Connection = new MySqlConnection();
                    break;
                case HDBType.Oracle:
                    Connection = new OracleConnection();
                    break;
            }
            Connection.ConnectionString = ConnectionString;
            Connection.Open();
        }

        public void AutoClose()
        {
            if (null != Connection && Connection.State != ConnectionState.Closed)
            {
                try { Connection.Close(); }
                catch { Connection.Dispose(); }
            }
            Connection.Dispose();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public IDbCommand CreateDBCommand(string __strCommandText)
        {
            IDbCommand command = null;
            switch (DataBaseType)
            {
                case HDBType.SqlServer:
                    command = new SqlCommand();
                    break;
                case HDBType.MySql:
                    command = new MySqlCommand();
                    break;
                case HDBType.Oracle:
                    command = new OracleCommand();
                    break;
            }
            command.CommandText = __strCommandText;
            command.Connection = Connection;
            return command;
        }

        public IDbDataAdapter CreateDBAdapter(string __strCommandText)
        {
            IDbDataAdapter dbDataAdapter = null;
            switch (DataBaseType)
            {
                case HDBType.SqlServer:
                    dbDataAdapter = new SqlDataAdapter();
                    break;
                case HDBType.MySql:
                    dbDataAdapter = new MySqlDataAdapter();
                    break;
                case HDBType.Oracle:
                    dbDataAdapter = new OracleDataAdapter();
                    break;
            }
            dbDataAdapter.SelectCommand = CreateDBCommand(__strCommandText);
            return dbDataAdapter;
        }

    }

    public enum HDBType
    {
        SqlServer = 0,
        Oracle = 1,
        MySql = 2 
    }
}
