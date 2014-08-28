using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Interfaces;
using System.Data.Common;
using System.Data;

namespace BlueSky.DataAccess
{
    public abstract class DbSession : IDbSession, IDisposable
    {
        private IDatabase _Database;
        private string _ConnectionString;
        private DbConnection _Connection;
        private bool _IsAutoClose = true;
        private DbTransaction _Trans;
        private int _TransCount;
        public IDatabase Database
        {
            get
            {
                if (null == this._Database)
                {
                    this._Database = this.CreateDatabase();
                }
                return this._Database;
            }
            set
            {
                this._Database = value;
            }
        }
        public DatabaseType DbType
        {
            get
            {
                return this._Database.DbType;
            }
        }
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(this._ConnectionString))
                {
                    this._ConnectionString = this.Database.ConnectionString;
                }
                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }
        public DbProviderFactory DbFactory
        {
            get
            {
                return this.Database.DbFactory;
            }
        }
        public DbConnection Connection
        {
            get
            {
                if (null == this._Connection)
                {
                    this._Connection = this.DbFactory.CreateConnection();
                    if (!string.IsNullOrEmpty(this.ConnectionString))
                    {
                        this._Connection.ConnectionString = this.ConnectionString;
                    }
                }
                return this._Connection;
            }
        }
        public DbTransaction Trans
        {
            get
            {
                return this._Trans;
            }
            set
            {
                this._Trans = value;
            }
        }
        public int TransCount
        {
            get
            {
                return this._TransCount;
            }
            set
            {
                this._TransCount = value;
            }
        }
        public bool IsAutoClose
        {
            get
            {
                return this._IsAutoClose;
            }
            set
            {
                this._IsAutoClose = value;
            }
        }
        public bool Opened
        {
            get 
            {
                return null != this.Connection && this.Connection.State != ConnectionState.Closed;
            }        
        }
        public void Open()
        {
            if (null != Connection && Connection.State == ConnectionState.Closed)
            {
                this.Connection.Open();
            }
        }
        public void Close()
        {
            if (null != this.Connection && this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }
        }
        public void AutoClose()
        {
            if (this.IsAutoClose && null != this.Connection && this.Opened)
            {
                this.Close();
            }
        }
        public void Dispose()
        {
            if (null != this.Connection && this.Opened)
            {
                this.Close();
            }
        }
        public int BeginTransaction()
        {
            this.TransCount++;
            if (this.TransCount >= 2)
            {
                return this.TransCount;
            }
            if (!this.Opened)
            {
                this.Open();
            }
            this.Trans = this.Connection.BeginTransaction();
            return this.TransCount;
        }
        public int Commit()
        {
            if (null == this.Trans || this.TransCount <= 0)
            {
                throw new Exception("当前未开启事务，不能进行提交操作！");
            }
            this.TransCount--;
            if (this.TransCount >= 1)
            {
                return this.TransCount;
            }
            this.Trans.Commit();
            this.Trans = null;
            this.AutoClose();
            return 0;
        }
        public int RollBack()
        {
            if (null == this.Trans || this.TransCount <= 0)
            {
                throw new Exception("当前未开启事务，不能进行回滚操作！");
            }
            this.TransCount--;
            if (this.TransCount >= 1)
            {
                return this.TransCount;
            }
            this.Trans.Rollback();
            this.Trans = null;
            this.AutoClose();
            return 0;
        }
        public virtual IDatabase CreateDatabase()
        {
            return this._Database = this.OnCreateDatabase();
        }
        public abstract IDatabase OnCreateDatabase();
        public DbCommand Command()
        {
            DbCommand cmd = this.DbFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            if (!this.Opened)
            {
                this.Open();
            }
            cmd.Connection = this.Connection;
            if (null != this.Trans)
            {
                cmd.Transaction = this.Trans;
            }
            return cmd;
        }
        public DbCommand Command(string _strSql, params DbParameter[] _Params)
        {
            DbCommand cmd = this.Command();
            cmd.CommandText = _strSql;
            if (null != _Params && _Params.Length > 0)
            {
                cmd.Parameters.AddRange(_Params);
            }
            return cmd;
        }
        public DbCommand Command(string _strSql, int _nTimeout, params DbParameter[] _Params)
        {
            DbCommand cmd = this.Command(_strSql, _Params);
            cmd.CommandTimeout = _nTimeout;
            return cmd;
        }
        public DbCommand Command(string _strSql, CommandType _cmdType, params DbParameter[] _Params)
        {
            DbCommand cmd = this.Command(_strSql, _Params);
            cmd.CommandType = _cmdType;
            return cmd;
        }
        public DbCommand Command(string _strSql, CommandType _cmdType, int _nTimeout, params DbParameter[] _Params)
        {
            DbCommand cmd = this.Command(_strSql, _cmdType, _Params);
            cmd.CommandTimeout = _nTimeout;
            return cmd;
        }
        public DataSet Query(string _strSql)
        {
            return Query(this.Command(_strSql, null));
        }
        public DataSet Query(string _strSql, params DbParameter[] _Params)
        {
            return Query(this.Command(_strSql, _Params));
        }
        public DataSet Query(string _strSql, int _nTimeout)
        {
            return Query(this.Command(_strSql, _nTimeout, null));
        }
        public DataSet Query(string _strSql, int _nTimeout, params DbParameter[] _Params)
        {
            return Query(this.Command(_strSql, _nTimeout, _Params));
        }
        public DataSet Query(string _strSql, CommandType _cmdType, params DbParameter[] _Params)
        {
            return Query(this.Command(_strSql, _cmdType, _Params));
        }
        public DataSet Query(string _strSql, CommandType _cmdType, int _nTimeout, params DbParameter[] _Params)
        {
            return Query(this.Command(_strSql, _cmdType, _nTimeout, _Params));
        }
        public DataSet Query(DbCommand _cmd)
        {
            DataSet ds = new DataSet();
            using (DbDataAdapter adapter = this.DbFactory.CreateDataAdapter())
            {
                adapter.SelectCommand = _cmd;
                try
                {
                    adapter.Fill(ds);
                }
                catch (Exception ee)
                {
                    
                }
                finally
                {
                    this.AutoClose();
                }
            }
            return ds;
        }
        public T ExecuteScale<T>(string _strSql)
        {
            return ExecuteScale<T>(this.Command(_strSql, null));
        }
        public T ExecuteScale<T>(string _strSql, params DbParameter[] _Params)
        {
            return ExecuteScale<T>(this.Command(_strSql, _Params));
        }
        public T ExecuteScale<T>(string _strSql, CommandType _cmdType, params DbParameter[] _Params)
        {
            return ExecuteScale<T>(this.Command(_strSql, _cmdType, _Params));
        }
        public T ExecuteScale<T>(DbCommand _cmd)
        {
            T result = default(T);
            try
            {
                object oValue = _cmd.ExecuteScalar();
                if (null == oValue || DBNull.Value == oValue)
                {
                    return result;
                }
                if (oValue is T)
                {
                    result = (T)oValue;
                }
                else
                {
                    result = (T)Convert.ChangeType(oValue, typeof(T));
                }
            }
            catch (Exception ee)
            {

            }
            finally
            {
                this.AutoClose();
            }
            return result;
        }
        public int Execute(string _strSql)
        {
            return this.Execute(this.Command(_strSql, null));
        }
        public int Execute(string _strSql, int _nTimeout)
        {
            return this.Execute(this.Command(_strSql, _nTimeout, null));
        }
        public int Execute(string _strSql, params DbParameter[] _Params)
        {
            return this.Execute(this.Command(_strSql, _Params));
        }
        public int Execute(string _strSql, int _nTimeout, params DbParameter[] _Params)
        {
            return this.Execute(this.Command(_strSql, _nTimeout, _Params));
        }
        public int Execute(string _strSql, CommandType _cmdType, params DbParameter[] _Params)
        {
            return this.Execute(this.Command(_strSql, _cmdType, _Params));
        }
        public int Execute(string _strSql, CommandType _cmdType, int _nTimeout, params DbParameter[] _Params)
        {
            return this.Execute(this.Command(_strSql, _cmdType, _nTimeout, _Params));
        }
        public int Execute(DbCommand _cmd)
        {
            int nEffective = 0;
            try
            {
                if (null != this.Trans)
                {
                    _cmd.Transaction = this.Trans;
                }
                nEffective = _cmd.ExecuteNonQuery();
            }
            catch (Exception ee)
            {

            }
            finally
            {
                this.AutoClose();
            }
            return nEffective;
        }
    }
}
