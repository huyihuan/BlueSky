using BlueSky.DataAccess;
using System;
using System.Data.Common;
namespace BlueSky.Interfaces
{
	public interface IDatabase
	{
		string ConnectionString
		{
			get;
			set;
		}
		string ConnectionName
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
			set;
		}
	}
}
