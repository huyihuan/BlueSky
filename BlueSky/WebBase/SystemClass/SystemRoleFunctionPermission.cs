using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemRoleFunctionPermission : IEntity
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
		public static SystemRoleFunctionPermission Get(int _nId)
		{
			SystemRoleFunctionPermission result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemRoleFunctionPermission>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemRoleFunctionPermission[] GetByFunctionId(int _nFunctionId)
		{
			SystemRoleFunctionPermission[] result;
			if (_nFunctionId <= 0)
			{
				result = null;
			}
			else
			{
				result = SystemRoleFunctionPermission.List("FunctionId=" + _nFunctionId);
			}
			return result;
		}
		public static SystemRoleFunctionPermission[] GetRoleFunctions(int _nRoleId)
		{
			SystemRoleFunctionPermission[] result;
			if (_nRoleId <= 0)
			{
				result = null;
			}
			else
			{
				result = SystemRoleFunctionPermission.List("RoleId=" + _nRoleId);
			}
			return result;
		}
		public static SystemRoleFunctionPermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemRoleFunctionPermission>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
		public static SystemRoleFunctionPermission[] List()
		{
			return EntityAccess<SystemRoleFunctionPermission>.Access.List();
		}
		public static SystemRoleFunctionPermission[] List(string _strFilter)
		{
            return EntityAccess<SystemRoleFunctionPermission>.Access.List(_strFilter);
		}
		public static void Delete(int _nId)
		{
			SystemRoleFunctionPermission oDel = SystemRoleFunctionPermission.Get(_nId);
			if (null != oDel)
			{
				SystemRoleActionPermission[] alActions = SystemRoleActionPermission.GetRoleActions(oDel.RoleId, oDel.FunctionId);
				int nCount = (alActions == null) ? 0 : alActions.Length;
				for (int i = 0; i < nCount; i++)
				{
					SystemRoleActionPermission.Delete(alActions[i].Id);
				}
				EntityAccess<SystemRoleFunctionPermission>.Access.Delete(oDel);
			}
		}
		public static int Save(SystemRoleFunctionPermission _Entity)
		{
			return EntityAccess<SystemRoleFunctionPermission>.Access.Save(_Entity);
		}
	}
}
