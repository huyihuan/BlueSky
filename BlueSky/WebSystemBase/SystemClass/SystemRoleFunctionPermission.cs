using System;
using System.Collections.Generic;
using System.Web;
using BlueSky.Interfaces;
using BlueSky.EntityAccess;

namespace WebSystemBase.SystemClass
{
    public class SystemRoleFunctionPermission : IEntity
    {
        public int Id;
        public int RoleId;
        public int FunctionId;

        #region IEntity

        public string GetTableName() { return "SystemRoleFunctionPermission"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemRoleFunctionPermission Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemRoleFunctionPermission oGet = new SystemRoleFunctionPermission();
            SystemRoleFunctionPermission[] alist = (SystemRoleFunctionPermission[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemRoleFunctionPermission[] GetByFunctionId(int _nFunctionId)
        {
            if (_nFunctionId <= 0)
                return null;
            SystemRoleFunctionPermission oGet = new SystemRoleFunctionPermission();
            SystemRoleFunctionPermission[] alist = (SystemRoleFunctionPermission[])HEntityCommon.HEntity(oGet).EntityList("FunctionId=" + _nFunctionId);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleFunctionPermission[] GetRoleFunctions(int _nRoleId)
        {
            if (_nRoleId <= 0)
                return null;
            SystemRoleFunctionPermission oGet = new SystemRoleFunctionPermission();
            oGet.RoleId = _nRoleId;
            SystemRoleFunctionPermission[] alist = (SystemRoleFunctionPermission[])HEntityCommon.HEntity(oGet).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleFunctionPermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemRoleFunctionPermission oList = new SystemRoleFunctionPermission();
            SystemRoleFunctionPermission[] alist = (SystemRoleFunctionPermission[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleFunctionPermission[] List()
        {
            SystemRoleFunctionPermission[] alist = (SystemRoleFunctionPermission[])HEntityCommon.HEntity(new SystemRoleFunctionPermission()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleFunctionPermission[] List(string _strFilter)
        {
            SystemRoleFunctionPermission[] alist = (SystemRoleFunctionPermission[])HEntityCommon.HEntity(new SystemRoleFunctionPermission()).EntityList(_strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void Delete(int _nId)
        {
            SystemRoleFunctionPermission oDel = Get(_nId);
            if (null == oDel)
                return;

            //1、删除该角色Funcion所包含的Action
            SystemRoleActionPermission[] alActions = SystemRoleActionPermission.GetRoleActions(oDel.RoleId, oDel.FunctionId);
            int nCount = null == alActions ? 0 : alActions.Length;
            for (int i = 0; i < nCount; i++)
                SystemRoleActionPermission.Delete(alActions[i].Id);

            HEntityCommon.HEntity(oDel).EntityDelete();
        }

    }
}
