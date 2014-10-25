using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemUserActionPermission : IEntity
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
		public int ActionId
		{
			get;
			set;
		}
		public static SystemUserActionPermission Get(int _nId)
		{
			SystemUserActionPermission result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemUserActionPermission>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemUserActionPermission[] Get(int _nUserId, int _nFunctionId)
		{
			SystemUserActionPermission[] result;
			if (_nUserId <= 0 || _nFunctionId <= 0)
			{
				result = null;
			}
			else
			{
				SystemUserActionPermission oGet = new SystemUserActionPermission();
				oGet.UserId = _nUserId;
				oGet.FunctionId = _nFunctionId;
				result = EntityAccess<SystemUserActionPermission>.Access.List(oGet);
			}
			return result;
		}
		public static SystemUserActionPermission[] GetByActionId(int _nActionId)
		{
			SystemUserActionPermission[] result;
			if (_nActionId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemUserActionPermission>.Access.List("ActionId=" + _nActionId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemUserActionPermission oDel = SystemUserActionPermission.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemUserActionPermission>.Access.Delete(oDel);
			}
		}
		public static SystemUserActionPermission[] List()
		{
			return EntityAccess<SystemUserActionPermission>.Access.List();
		}
		public static SystemUserActionPermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemUserActionPermission>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
		public static int Save(SystemUserActionPermission _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemUserActionPermission>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
