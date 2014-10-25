using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemRoleModulePermission : IEntity
	{
		[EntityField(FieldName = "Id", IsPrimaryKey = true)]
		public int Id
		{
			get;
			set;
		}
		public int RoleId
		{
			get;
			set;
		}
		public string ModuleId
		{
			get;
			set;
		}
		public static SystemRoleModulePermission Get(int _nId)
		{
			SystemRoleModulePermission result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemRoleModulePermission>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemRoleModulePermission[] GetByModuleId(int _nModuleId)
		{
			SystemRoleModulePermission[] result;
			if (_nModuleId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemRoleModulePermission>.Access.List("ModuleId=" + _nModuleId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemRoleModulePermission oDel = SystemRoleModulePermission.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemRoleModulePermission>.Access.Delete(oDel);
			}
		}
		public static SystemRoleModulePermission[] List()
		{
			return EntityAccess<SystemRoleModulePermission>.Access.List();
		}
		public static SystemRoleModulePermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemRoleModulePermission>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
	}
}
