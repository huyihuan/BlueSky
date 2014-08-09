using BlueSky.DataAccess;
using System;
using System.Data;
using System.Data.Common;
namespace BlueSky.Interfaces
{
	public interface IDbSession
	{
		string ConnectionString
		{
			get;
            set;
		}
		DatabaseType DbType
		{
			get;
		}
		DbProviderFactory DbFactory
		{
			get;
		}
		IDatabase Database
		{
			get;
			set;
		}
		DbConnection Connection
		{
			get;
		}
		DbTransaction Trans
		{
			get;
			set;
		}
		int TransCount
		{
			get;
			set;
		}
		bool IsAutoClose
		{
			get;
			set;
		}
		bool Opened
		{
			get;
		}
		void Open();
		void Close();
		void AutoClose();
		int BeginTransaction();
		int Commit();
		int RollBack();
		DbCommand Command();
		DbCommand Command(string _strSql, params DbParameter[] _Params);
		DbCommand Command(string _strSql, int _nTimeout, params DbParameter[] _Params);
		DbCommand Command(string _strSql, CommandType _cmdType, params DbParameter[] _Params);
		DbCommand Command(string _strSql, CommandType _cmdType, int _nTimeout, params DbParameter[] _Params);
		DataSet Query(string _strSql);
		DataSet Query(string _strSql, params DbParameter[] _Params);
		DataSet Query(string _strSql, int _nTimeout);
		DataSet Query(string _strSql, int _nTimeout, params DbParameter[] _Params);
		DataSet Query(string _strSql, CommandType _cmdType, params DbParameter[] _Params);
		DataSet Query(string _strSql, CommandType _cmdType, int _nTimeout, params DbParameter[] _Params);
		DataSet Query(DbCommand _cmd);
		T ExecuteScale<T>(string _strSql);
		T ExecuteScale<T>(string _strSql, params DbParameter[] _Params);
		T ExecuteScale<T>(string _strSql, CommandType _cmdType, params DbParameter[] _Params);
		T ExecuteScale<T>(DbCommand _cmd);
		int Execute(string _strSql);
		int Execute(string _strSql, int _nTimeout);
		int Execute(string _strSql, int _nTimeout, params DbParameter[] _Params);
		int Execute(string _strSql, CommandType _cmdType, params DbParameter[] _Params);
		int Execute(string _strSql, CommandType _cmdType, int _nTimeout, params DbParameter[] _Params);
		int Execute(DbCommand _cmd);
	}
}
