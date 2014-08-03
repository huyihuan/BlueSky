using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemRoleActionPermission : IEntity
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
		public int FunctionId
		{
			get;
			set;
		}
		public int ActionId
		{
			get;
			set;
		}
		public static SystemRoleActionPermission Get(int _nId)
		{
			SystemRoleActionPermission result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemRoleActionPermission>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemRoleActionPermission[] GetByActionId(int _nActionId)
		{
			SystemRoleActionPermission[] result;
			if (_nActionId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemRoleActionPermission>.Access.List("ActionId=" + _nActionId);
			}
			return result;
		}
		public static SystemRoleActionPermission[] GetRoleActions(int _nRoleId)
		{
			SystemRoleActionPermission[] result;
			if (_nRoleId <= 0)
			{
				result = null;
			}
			else
			{
				SystemRoleActionPermission oGet = new SystemRoleActionPermission();
				oGet.RoleId = _nRoleId;
				result = EntityAccess<SystemRoleActionPermission>.Access.List(oGet);
			}
			return result;
		}
		public static SystemRoleActionPermission[] GetRoleActions(int _nRoleId, int _nFunctionId)
		{
			SystemRoleActionPermission[] result;
			if (_nRoleId <= 0 || _nFunctionId <= 0)
			{
				result = null;
			}
			else
			{
				SystemRoleActionPermission oGet = new SystemRoleActionPermission();
				oGet.RoleId = _nRoleId;
				oGet.FunctionId = _nFunctionId;
				result = EntityAccess<SystemRoleActionPermission>.Access.List(oGet);
			}
			return result;
		}
		public static SystemRoleActionPermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
			return EntityAccess<SystemRoleActionPermission>.Access.List(__strFilter, "", __nPageIndex, __nPageSize);
		}
		public static SystemRoleActionPermission[] List()
		{
			return EntityAccess<SystemRoleActionPermission>.Access.List();
		}
		public static void Delete(int _nId)
		{
			SystemRoleActionPermission oDel = SystemRoleActionPermission.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemRoleActionPermission>.Access.Delete(oDel);
			}
		}
		public static int Save(SystemRoleActionPermission _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemRoleActionPermission>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
