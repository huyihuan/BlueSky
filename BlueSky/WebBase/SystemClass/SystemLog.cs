using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
using BlueSky.DataAccess;
namespace WebBase.SystemClass
{
	[EntityAttribue(EnableCache = false, ConectionName="BlueSkyLog", DbType=DatabaseType.SqlServer)]
	public class SystemLog : IEntity
	{
		[EntityField(FieldName = "Id", IsPrimaryKey = true)]
		public int Id
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		public DateTime AccessTime
		{
			get;
			set;
		}
		public string AccessFunctionName
		{
			get;
			set;
		}
		public string AccessActionName
		{
			get;
			set;
		}
		public string AccessURL
		{
			get;
			set;
		}
		public string Remark
		{
			get;
			set;
		}
		public string UserName
		{
			get
			{
				UserInformation oItem = UserInformation.Get(this.UserId);
				if (null == oItem)
				{
					return "";
				}
                return oItem.UserName;
			}
		}
		public string FormattingAccessTime
		{
			get
			{
				return this.AccessTime.ToString("yyyy-MM-dd hh:mm:ss");
			}
		}
		public static SystemLog Get(int _nId)
		{
			SystemLog result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemLog>.Access.Get(_nId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemLog oDel = SystemLog.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemLog>.Access.Delete(oDel);
			}
		}
		public static SystemLog[] List()
		{
			return SystemLog.List("");
		}
		public static SystemLog[] List(string _strFilter)
		{
			return EntityAccess<SystemLog>.Access.List(_strFilter);
		}
		public static SystemLog[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemLog>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
		public static int Save(SystemLog _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemLog>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
