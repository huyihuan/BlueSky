using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using DataBase;
using System.Web.UI.WebControls;

namespace WebSystemBase.SystemClass
{
    public class SystemFunction : DataBase.Interface.IEntity, IComparer<SystemFunction>
    {
        public int Id;
        public string Name;
        public string Key;
        public int ModuleId;
        public int ParentId;
        public string IconName;
        public string Description;
        public int OrderId;
        public int Level; //功能的深度（默认值：1 即第一级，1,2,3....）

        #region IEntity

        public string GetTableName() { return "SystemFunction"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        #region IComparer

        public int Compare(SystemFunction x, SystemFunction y)
        {
            return x.OrderId - y.OrderId;
        }

        #endregion

        public static SystemFunction Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemFunction oGet = new SystemFunction();
            SystemFunction[] alist = (SystemFunction[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemFunction Get(string _strKey)
        {
            if (string.IsNullOrEmpty(_strKey))
                return null;
            SystemFunction oGet = new SystemFunction();
            SystemFunction[] alist = (SystemFunction[])HEntityCommon.HEntity(oGet).EntityList(string.Format("[Key]='{0}'", _strKey));
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), "Key", _strKey));
            return alist[0];
        }

        public static SystemFunction[] GetUserFunctin(int _nUserId)
        {
            if (_nUserId <= 0)
                return null;
            List<string> ltFnId = new List<string>();

            //1、获取用户所属角色功能列表
            SystemUserRole[] alUserRoles = SystemUserRole.GetUserRoles(_nUserId);
            if (null != alUserRoles && alUserRoles.Length > 0)
            {
                List<string> ltUserRoleIds = new List<string>();
                foreach (SystemUserRole oRole in alUserRoles)
                    ltUserRoleIds.Add(oRole.RoleId + "");

                SystemRoleFunctionPermission[] alRoleFunction = SystemRoleFunctionPermission.List(string.Format("RoleId in ({0})", string.Join(",", ltUserRoleIds.ToArray())));
                if (null != alRoleFunction && alRoleFunction.Length > 0)
                {
                    foreach (SystemRoleFunctionPermission oPermission in alRoleFunction)
                            ltFnId.Add(oPermission.FunctionId + "");
                }
            }

            //2、获取用户自定义功能列表
            SystemUserFunctionPermission[] alUserFunction = SystemUserFunctionPermission.GetUserFunctionPermission(_nUserId);
            if (null != alUserFunction && alUserFunction.Length > 0)
            {
                foreach (SystemUserFunctionPermission oPermission in alUserFunction)
                {
                    if (!ltFnId.Contains(oPermission.FunctionId + ""))
                        ltFnId.Add(oPermission.FunctionId + "");
                }
            }

            if (ltFnId.Count == 0)
                return null;
            string strFilter = string.Format("Id in ({0})",string.Join(",",ltFnId.ToArray()));
            //按照OrderId排序
            SystemFunction[] alFunctions = List(strFilter);
            List<SystemFunction> ltOrder = new List<SystemFunction>(alFunctions);
            ltOrder.Sort(new SystemFunction());
            return ltOrder.ToArray();
        }

        public static void Delete(int _nId)
        {
            SystemFunction oDel = Get(_nId);
            if (null == oDel)
                return;
            //1、删除角色功能关联表
            SystemRoleFunctionPermission[] alRoleFns = SystemRoleFunctionPermission.GetByFunctionId(_nId);
            if (null != alRoleFns && alRoleFns.Length > 0)
            {
                foreach (SystemRoleFunctionPermission permission in alRoleFns)
                    SystemRoleFunctionPermission.Delete(permission.Id);
            }

            //2、删除用户功能关联表
            SystemUserFunctionPermission[] alUserFns = SystemUserFunctionPermission.GetByFunctionId(_nId);
            if (null != alUserFns && alUserFns.Length > 0)
            {
                foreach (SystemUserFunctionPermission permission in alUserFns)
                    SystemUserFunctionPermission.Delete(permission.Id);
            }

            //3、删除功能操作
            SystemAction[] alAcions = SystemAction.GetFunctionAction(_nId);
            if (null != alAcions && alAcions.Length > 0)
            {
                foreach (SystemAction action in alAcions)
                    SystemAction.Delete(action.Id);
            }

            //4、删除子功能
            SystemFunction[] alSonFns = GetFunctions(_nId, false);
            if (null != alSonFns && alSonFns.Length > 0)
            {
                foreach (SystemFunction fn in alSonFns)
                    Delete(fn.Id);
            }

            //5、删除父级功能
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static SystemFunction[] GetFunctions(int _nParentId, bool _bIncludeAllChildren)
        {
            string strFilter = "ParentId =" + _nParentId;
            if(_bIncludeAllChildren)
                strFilter = "ParentId >=" + _nParentId;
            return List(strFilter);
        }

        public static Hashtable GetParentIdToCount(SystemFunction[] _alFunctions)
        {
            Hashtable htParentIdToCount = new Hashtable();
            if (null == _alFunctions || _alFunctions.Length == 0)
                return htParentIdToCount;
            int nCount = _alFunctions.Length;
            for (int i = 0; i < nCount; i++)
            {
                int nParentId = _alFunctions[i].ParentId;
                int nChildrenCount = 0;
                for (int j = 0; j < nCount; j++)
                {
                    if (nParentId == _alFunctions[j].ParentId)
                        nChildrenCount++;
                }
                htParentIdToCount[nParentId] = nChildrenCount;
            }
            return htParentIdToCount;
        }

        public static SystemFunction[] List(string _strFilter)
        {
            SystemFunction[] alist = (SystemFunction[])DataBase.HEntityCommon.HEntity(new SystemFunction()).EntityList(_strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static int Save(SystemFunction _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }
    }
}
