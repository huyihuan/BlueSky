using System;
using System.Collections.Generic;
using System.Text;
using DataBase;

namespace WebSystemBase.SystemClass
{
    public class SystemUserActionPermission : DataBase.Interface.IEntity
    {

        public int Id;
        public int UserId;
        public int FunctionId;
        public int ActionId;

        #region IEntity

        public string GetTableName() { return "SystemUserActionPermission"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemUserActionPermission Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemUserActionPermission oGet = new SystemUserActionPermission();
            SystemUserActionPermission[] alist = (SystemUserActionPermission[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemUserActionPermission[] Get(int _nUserId, int _nFunctionId)
        {
            if (_nUserId <= 0 || _nFunctionId <= 0)
                return null;
            SystemUserActionPermission oGet = new SystemUserActionPermission();
            oGet.UserId = _nUserId;
            oGet.FunctionId = _nFunctionId;
            SystemUserActionPermission[] alist = (SystemUserActionPermission[])HEntityCommon.HEntity(oGet).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemUserActionPermission[] GetByActionId(int _nActionId)
        {
            if (_nActionId <= 0)
                return null;
            SystemUserActionPermission oGet = new SystemUserActionPermission();
            SystemUserActionPermission[] alist = (SystemUserActionPermission[])HEntityCommon.HEntity(oGet).EntityList("ActionId=" + _nActionId);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void Delete(int _nId)
        {
            SystemUserActionPermission oDel = Get(_nId);
            if (null == oDel)
                return;
            DataBase.HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static SystemUserActionPermission[] List()
        {
            SystemUserActionPermission[] alist = (SystemUserActionPermission[])DataBase.HEntityCommon.HEntity(new SystemUserActionPermission()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemUserActionPermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemUserActionPermission oList = new SystemUserActionPermission();
            SystemUserActionPermission[] alist = (SystemUserActionPermission[])DataBase.HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static int Save(SystemUserActionPermission _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }
    }
}
