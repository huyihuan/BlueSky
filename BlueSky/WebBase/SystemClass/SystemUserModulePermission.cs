using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemUserModulePermission : IEntity
	{
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
		public int ModuleId
		{
			get;
			set;
		}
		public static SystemUserModulePermission Get(int _nId)
		{
			SystemUserModulePermission result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemUserModulePermission>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemUserModulePermission[] GetByModuleId(int _nModuleId)
		{
			SystemUserModulePermission[] result;
			if (_nModuleId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemUserModulePermission>.Access.List("ModuleId=" + _nModuleId);
			}
			return result;
		}
		public static SystemUserModulePermission[] GetUserModulePermission(int _nUserId)
		{
			SystemUserModulePermission[] result;
			if (_nUserId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemUserModulePermission>.Access.List("UserId=" + _nUserId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemUserModulePermission oDel = SystemUserModulePermission.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemUserModulePermission>.Access.Delete(oDel);
			}
		}
		public static SystemUserModulePermission[] List()
		{
			return EntityAccess<SystemUserModulePermission>.Access.List();
		}
		public static SystemUserModulePermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemUserModulePermission>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
	}
}
