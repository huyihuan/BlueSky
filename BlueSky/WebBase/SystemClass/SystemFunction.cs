using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
namespace WebBase.SystemClass
{
	public class SystemFunction : IEntity, IComparer<SystemFunction>
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
		public int ModuleId
		{
			get;
			set;
		}
		public int ParentId
		{
			get;
			set;
		}
		public string IconName
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public int OrderId
		{
			get;
			set;
		}
		public int Level
		{
			get;
			set;
		}
		public int Compare(SystemFunction x, SystemFunction y)
		{
			return x.OrderId - y.OrderId;
		}
		public static SystemFunction Get(int _nId)
		{
			SystemFunction result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemFunction>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemFunction Get(string _strKey)
		{
			SystemFunction result;
			if (string.IsNullOrEmpty(_strKey))
			{
				result = null;
			}
			else
			{
				SystemFunction[] alist = EntityAccess<SystemFunction>.Access.List(string.Format("[Key]='{0}'", _strKey));
				if (alist == null || alist.Length == 0)
				{
					result = null;
				}
				else
				{
					if (alist.Length > 1)
					{
						throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", EntityAccess<SystemFunction>.Meta.EntityName, "Key", _strKey));
					}
					result = alist[0];
				}
			}
			return result;
		}
		public static SystemFunction[] GetUserFunctin(int _nUserId)
		{
			SystemFunction[] result;
			if (_nUserId <= 0)
			{
				result = null;
			}
			else
			{
				List<string> ltFnId = new List<string>();
				SystemUserRole[] alUserRoles = SystemUserRole.GetUserRoles(_nUserId);
				if (alUserRoles != null && alUserRoles.Length > 0)
				{
					List<string> ltUserRoleIds = new List<string>();
					SystemUserRole[] array = alUserRoles;
					for (int i = 0; i < array.Length; i++)
					{
						SystemUserRole oRole = array[i];
						ltUserRoleIds.Add(string.Concat(oRole.RoleId));
					}
					SystemRoleFunctionPermission[] alRoleFunction = SystemRoleFunctionPermission.List(string.Format("RoleId in ({0})", string.Join(",", ltUserRoleIds.ToArray())));
					if (alRoleFunction != null && alRoleFunction.Length > 0)
					{
						SystemRoleFunctionPermission[] array2 = alRoleFunction;
						for (int i = 0; i < array2.Length; i++)
						{
							SystemRoleFunctionPermission oPermission = array2[i];
							ltFnId.Add(string.Concat(oPermission.FunctionId));
						}
					}
				}
				SystemUserFunctionPermission[] alUserFunction = SystemUserFunctionPermission.GetUserFunctionPermission(_nUserId);
				if (alUserFunction != null && alUserFunction.Length > 0)
				{
					SystemUserFunctionPermission[] array3 = alUserFunction;
					for (int i = 0; i < array3.Length; i++)
					{
						SystemUserFunctionPermission oPermission2 = array3[i];
						if (!ltFnId.Contains(string.Concat(oPermission2.FunctionId)))
						{
							ltFnId.Add(string.Concat(oPermission2.FunctionId));
						}
					}
				}
				if (ltFnId.Count == 0)
				{
					result = null;
				}
				else
				{
					string strFilter = string.Format("Id in ({0})", string.Join(",", ltFnId.ToArray()));
					SystemFunction[] alFunctions = SystemFunction.List(strFilter);
					List<SystemFunction> ltOrder = new List<SystemFunction>(alFunctions);
					ltOrder.Sort(new SystemFunction());
					result = ltOrder.ToArray();
				}
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemFunction oDel = SystemFunction.Get(_nId);
			if (null != oDel)
			{
				SystemRoleFunctionPermission[] alRoleFns = SystemRoleFunctionPermission.GetByFunctionId(_nId);
				if (alRoleFns != null && alRoleFns.Length > 0)
				{
                    for (int i = 0; i < alRoleFns.Length; i++)
					{
                        SystemRoleFunctionPermission.Delete(alRoleFns[i].Id);
					}
				}
				SystemUserFunctionPermission[] alUserFns = SystemUserFunctionPermission.GetByFunctionId(_nId);
				if (alUserFns != null && alUserFns.Length > 0)
				{
					for (int i = 0; i < alUserFns.Length; i++)
					{
						SystemUserFunctionPermission.Delete(alUserFns[i].Id);
					}
				}
				SystemAction[] alAcions = SystemAction.GetFunctionAction(_nId);
				if (alAcions != null && alAcions.Length > 0)
				{
                    for (int i = 0; i < alAcions.Length; i++)
					{
                        SystemAction.Delete(alAcions[i].Id);
					}
				}
				SystemFunction[] alSonFns = SystemFunction.GetFunctions(_nId, false);
				if (alSonFns != null && alSonFns.Length > 0)
				{
                    for (int i = 0; i < alSonFns.Length; i++)
					{
                        SystemFunction.Delete(alSonFns[i].Id);
					}
				}
				EntityAccess<SystemFunction>.Access.Delete(oDel);
			}
		}
		public static SystemFunction[] GetFunctions(int _nParentId, bool _bIncludeAllChildren)
		{
			string strFilter = "ParentId =" + _nParentId;
			if (_bIncludeAllChildren)
			{
				strFilter = "ParentId >=" + _nParentId;
			}
			return SystemFunction.List(strFilter);
		}
		public static Hashtable GetParentIdToCount(SystemFunction[] _alFunctions)
		{
			Hashtable htParentIdToCount = new Hashtable();
			Hashtable result;
			if (_alFunctions == null || _alFunctions.Length == 0)
			{
				result = htParentIdToCount;
			}
			else
			{
				int nCount = _alFunctions.Length;
				for (int i = 0; i < nCount; i++)
				{
					int nParentId = _alFunctions[i].ParentId;
					int nChildrenCount = 0;
					for (int j = 0; j < nCount; j++)
					{
						if (nParentId == _alFunctions[j].ParentId)
						{
							nChildrenCount++;
						}
					}
					htParentIdToCount[nParentId] = nChildrenCount;
				}
				result = htParentIdToCount;
			}
			return result;
		}
		public static SystemFunction[] List(string _strFilter)
		{
			return EntityAccess<SystemFunction>.Access.List(_strFilter);
		}
		public static int Save(SystemFunction _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemFunction>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
