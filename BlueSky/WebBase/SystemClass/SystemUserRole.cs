using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemUserRole : IEntity
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
		public int RoleId
		{
			get;
			set;
		}
		public string PropertyRoleName
		{
			get
			{
				SystemRole oRole = SystemRole.Get(this.RoleId);
				return (oRole == null) ? "" : oRole.Name;
			}
		}
		public static SystemUserRole Get(int _nId)
		{
			SystemUserRole result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemUserRole>.Access.Get(_nId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemUserRole oDel = SystemUserRole.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemUserRole>.Access.Delete(oDel);
			}
		}
		public static int Save(SystemUserRole _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemUserRole>.Access.Save(_Entity);
			}
			return result;
		}
		public static SystemUserRole[] GetUserRoles(int _nUserId)
		{
			return EntityAccess<SystemUserRole>.Access.List("UserId=" + _nUserId);
		}
        public static SystemUserRole[] GetRoleUsers(int _nRoleId)
        {
            return EntityAccess<SystemUserRole>.Access.List("RoleId=" + _nRoleId);
        }
		public static void DeleteUserRoles(int _nUserId)
		{
			SystemUserRole[] alist = SystemUserRole.GetUserRoles(_nUserId);
			if (null != alist)
			{
				int nCount = alist.Length;
				for (int i = 0; i < nCount; i++)
				{
					SystemUserRole.Delete(alist[i].Id);
				}
			}
		}
	}
}
