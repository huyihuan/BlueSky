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
using System.Collections;

namespace BlueSky.DataAccess
{
    public class HDataBase : System.IDisposable
    {
        private static string _strConnectionString = string.Empty;
        private static HDBType _DatabaseType = HDBType.SqlServer;
        private IDbConnection _Connection;
        private static Hashtable htDBType;

        static HDataBase()
        {
            //初始化数据库类型
            htDBType = new Hashtable();
            htDBType["sqlserver"] = HDBType.SqlServer;
            htDBType["mysql"] = HDBType.MySql;
            htDBType["oracle"] = HDBType.Oracle;

            //初始化数据库连接字符串，以及数据库类型
            _strConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            string strDBType = ConfigurationManager.AppSettings["DbType"];
            if (string.IsNullOrEmpty(strDBType))
                strDBType = "sqlserver";
            if (!htDBType.ContainsKey(strDBType))
                throw new Exception(string.Format("Unkown Database Type \"{0}\"", strDBType));
            _DatabaseType = (HDBType)htDBType[strDBType];
        }

        public HDataBase()
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

        public string ConnectionString
        {
            get { return _strConnectionString; }
            set { _strConnectionString = value; }
        }
        public HDBType DatabaseType
        {
            get { return _DatabaseType; }
            set { _DatabaseType = value; }
        }
        public IDbConnection Connection
        {
            get { return _Connection; }
            set { _Connection = value; }
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

        public IDbCommand CreateDBCommand(string _strCommandText)
        {
            IDbCommand command = null;
            switch (DatabaseType)
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
            command.CommandText = _strCommandText;
            command.Connection = Connection;
            return command;
        }

        public IDbDataAdapter CreateDBAdapter(string __strCommandText)
        {
            IDbDataAdapter dbDataAdapter = null;
            switch (DatabaseType)
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
