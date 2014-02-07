using System;
using System.Collections.Generic;
using System.Web;
using DataBase;

namespace WebSystemBase.SystemClass
{
    public class SystemRoleActionPermission : DataBase.Interface.IEntity
    {
        public int Id;
        public int RoleId;
        public int FunctionId;
        public int ActionId;

        #region IEntity

        public string GetTableName() { return "SystemRoleActionPermission"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemRoleActionPermission Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemRoleActionPermission oGet = new SystemRoleActionPermission();
            SystemRoleActionPermission[] alist = (SystemRoleActionPermission[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemRoleActionPermission[] GetByActionId(int _nActionId)
        {
            if (_nActionId <= 0)
                return null;
            SystemRoleActionPermission oGet = new SystemRoleActionPermission();
            SystemRoleActionPermission[] alist = (SystemRoleActionPermission[])HEntityCommon.HEntity(oGet).EntityList("ActionId=" + _nActionId);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleActionPermission[] GetRoleActions(int _nRoleId)
        {
            if (_nRoleId <= 0)
                return null;
            SystemRoleActionPermission oGet = new SystemRoleActionPermission();
            oGet.RoleId = _nRoleId;
            SystemRoleActionPermission[] alist = (SystemRoleActionPermission[])HEntityCommon.HEntity(oGet).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleActionPermission[] GetRoleActions(int _nRoleId, int _nFunctionId)
        {
            if (_nRoleId <= 0 || _nFunctionId <= 0)
                return null;
            SystemRoleActionPermission oGet = new SystemRoleActionPermission();
            oGet.RoleId = _nRoleId;
            oGet.FunctionId = _nFunctionId;
            SystemRoleActionPermission[] alist = (SystemRoleActionPermission[])HEntityCommon.HEntity(oGet).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleActionPermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemRoleActionPermission oList = new SystemRoleActionPermission();
            SystemRoleActionPermission[] alist = (SystemRoleActionPermission[])DataBase.HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleActionPermission[] List()
        {
            SystemRoleActionPermission[] alist = (SystemRoleActionPermission[])DataBase.HEntityCommon.HEntity(new SystemRole()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void Delete(int _nId)
        {
            SystemRoleActionPermission oDel = Get(_nId);
            if (null == oDel)
                return;
            DataBase.HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static int Save(SystemRoleActionPermission _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }
    }
}
