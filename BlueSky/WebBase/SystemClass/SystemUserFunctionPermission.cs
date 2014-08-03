using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemUserFunctionPermission : IEntity
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
		public int FunctionId
		{
			get;
			set;
		}
		public static SystemUserFunctionPermission Get(int _nId)
		{
			SystemUserFunctionPermission result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemUserFunctionPermission>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemUserFunctionPermission[] GetByFunctionId(int _nFunctionId)
		{
			SystemUserFunctionPermission[] result;
			if (_nFunctionId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemUserFunctionPermission>.Access.List("FunctionId=" + _nFunctionId);
			}
			return result;
		}
		public static SystemUserFunctionPermission[] GetUserFunctionPermission(int _nUserId)
		{
			SystemUserFunctionPermission[] result;
			if (_nUserId <= 0)
			{
				result = null;
			}
			else
			{
				SystemUserFunctionPermission[] alist = SystemUserFunctionPermission.List("UserId=" + _nUserId);
				result = alist;
			}
			return result;
		}
		public static SystemUserFunctionPermission[] GetUserFunctionPermission(int _nUserId, int _nModuleId)
		{
			SystemUserFunctionPermission[] result;
			if (_nUserId <= 0 || _nModuleId <= 0)
			{
				result = null;
			}
			else
			{
				string strFilter = string.Format("UserId = {0} and FunctionId in (select Id from SystemFunction where ModuleId = {1})", _nUserId, _nModuleId);
				SystemUserFunctionPermission[] alist = SystemUserFunctionPermission.List(strFilter);
				result = alist;
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemUserFunctionPermission oDel = SystemUserFunctionPermission.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemUserFunctionPermission>.Access.Delete(oDel);
			}
		}
		public static SystemUserFunctionPermission[] List()
		{
			return EntityAccess<SystemUserFunctionPermission>.Access.List();
		}
		public static SystemUserFunctionPermission[] List(string _strFilter)
		{
			return EntityAccess<SystemUserFunctionPermission>.Access.List(_strFilter);
		}
		public static SystemUserFunctionPermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
			return EntityAccess<SystemUserFunctionPermission>.Access.List(__strFilter, "", __nPageIndex, __nPageSize);
		}
		public static int Save(SystemUserFunctionPermission _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemUserFunctionPermission>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
