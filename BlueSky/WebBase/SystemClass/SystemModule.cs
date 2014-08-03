using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemModule : IEntity
	{
		[EntityField(FieldName = "Id", IsPrimaryKey = true)]
		public int Id
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public string Key
		{
			get;
			set;
		}
		public string Controller
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public string IconName
		{
			get;
			set;
		}
		public int OrderId
		{
			get;
			set;
		}
		public static SystemModule Get(int _nId)
		{
			SystemModule result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemModule>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemModule Get(string _strKey)
		{
			SystemModule result;
			if (string.IsNullOrEmpty(_strKey))
			{
				result = null;
			}
			else
			{
				SystemModule[] alist = EntityAccess<SystemModule>.Access.List(string.Format("[Key]='{0}'", _strKey));
				if (alist == null || alist.Length == 0)
				{
					result = null;
				}
				else
				{
					if (alist.Length > 1)
					{
						throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", EntityAccess<SystemModule>.Meta.EntityName, "Key", _strKey));
					}
					result = alist[0];
				}
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemModule oDel = SystemModule.Get(_nId);
			if (null != oDel)
			{
				SystemRoleModulePermission[] alRoleModule = SystemRoleModulePermission.GetByModuleId(_nId);
				if (alRoleModule != null && alRoleModule.Length > 0)
				{
					SystemRoleModulePermission[] array = alRoleModule;
					for (int i = 0; i < array.Length; i++)
					{
						SystemRoleModulePermission permission = array[i];
						SystemRoleModulePermission.Delete(permission.Id);
					}
				}
				SystemUserModulePermission[] alUserModule = SystemUserModulePermission.GetByModuleId(_nId);
				if (alUserModule != null && alUserModule.Length > 0)
				{
					SystemUserModulePermission[] array2 = alUserModule;
					for (int i = 0; i < array2.Length; i++)
					{
						SystemUserModulePermission permission2 = array2[i];
						SystemUserModulePermission.Delete(permission2.Id);
					}
				}
				SystemFunction rootFunction = SystemFunction.Get(oDel.Key);
				if (null != rootFunction)
				{
					SystemFunction.Delete(rootFunction.Id);
				}
				EntityAccess<SystemModule>.Access.Delete(oDel);
			}
		}
		public static bool Exist(string _strKey)
		{
			bool result;
			if (string.IsNullOrEmpty(_strKey))
			{
				result = true;
			}
			else
			{
				int nExistCount = EntityAccess<SystemModule>.Access.Count(string.Format("[Key]='{0}'", _strKey));
				if (nExistCount > 1)
				{
					throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", EntityAccess<SystemModule>.Meta.EntityName, "Key", _strKey));
				}
				result = (nExistCount == 1);
			}
			return result;
		}
		public static SystemModule[] List()
		{
			return EntityAccess<SystemModule>.Access.List();
		}
		public static SystemModule[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
			return EntityAccess<SystemModule>.Access.List(__strFilter, "", __nPageIndex, __nPageSize);
		}
		public static int Save(SystemModule _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				int nModuleId = EntityAccess<SystemModule>.Access.Save(_Entity);
				if (nModuleId <= 0)
				{
					result = -1;
				}
				else
				{
					SystemFunction rootFn = SystemFunction.Get(_Entity.Key);
					if (null == rootFn)
					{
						rootFn = new SystemFunction();
						rootFn.Key = _Entity.Key;
						rootFn.ModuleId = nModuleId;
						rootFn.Level = 1;
						rootFn.ParentId = -1;
					}
					rootFn.Name = _Entity.Name;
					rootFn.IconName = _Entity.IconName;
					rootFn.Description = _Entity.Description;
					rootFn.OrderId = _Entity.OrderId;
					SystemFunction.Save(rootFn);
					result = nModuleId;
				}
			}
			return result;
		}
	}
}
