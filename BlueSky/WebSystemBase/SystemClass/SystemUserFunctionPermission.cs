using System;
using System.Collections.Generic;
using System.Text;
using DataBase;

namespace WebSystemBase.SystemClass
{
    public class SystemUserFunctionPermission : DataBase.Interface.IEntity
    {

        public int Id;
        public int UserId;
        public int FunctionId;

        #region IEntity

        public string GetTableName() { return "SystemUserFunctionPermission"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemUserFunctionPermission Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemUserFunctionPermission oGet = new SystemUserFunctionPermission();
            SystemUserFunctionPermission[] alist = (SystemUserFunctionPermission[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemUserFunctionPermission[] GetByFunctionId(int _nFunctionId)
        {
            if (_nFunctionId <= 0)
                return null;
            SystemUserFunctionPermission oGet = new SystemUserFunctionPermission();
            SystemUserFunctionPermission[] alist = (SystemUserFunctionPermission[])HEntityCommon.HEntity(oGet).EntityList("FunctionId=" + _nFunctionId);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemUserFunctionPermission[] GetUserFunctionPermission(int _nUserId)
        {
            if (_nUserId <= 0)
                return null;
            SystemUserFunctionPermission[] alist = List("UserId=" + _nUserId);
            return alist;
        }

        public static SystemUserFunctionPermission[] GetUserFunctionPermission(int _nUserId, int _nModuleId)
        {
            if (_nUserId <= 0 || _nModuleId <= 0)
                return null;
            string strFilter = string.Format("UserId = {0} and FunctionId in (select Id from SystemFunction where ModuleId = {1})", _nUserId, _nModuleId);
            SystemUserFunctionPermission[] alist = List(strFilter);
            return alist;
        }

        public static void Delete(int _nId)
        {
            SystemUserFunctionPermission oDel = Get(_nId);
            if (null == oDel)
                return;
            DataBase.HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static SystemUserFunctionPermission[] List()
        {
            SystemUserFunctionPermission[] alist = (SystemUserFunctionPermission[])DataBase.HEntityCommon.HEntity(new SystemUserFunctionPermission()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemUserFunctionPermission[] List(string _strFilter)
        {
            SystemUserFunctionPermission[] alist = (SystemUserFunctionPermission[])DataBase.HEntityCommon.HEntity(new SystemUserFunctionPermission()).EntityList(_strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemUserFunctionPermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemUserFunctionPermission oList = new SystemUserFunctionPermission();
            SystemUserFunctionPermission[] alist = (SystemUserFunctionPermission[])DataBase.HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static int Save(SystemUserFunctionPermission _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }
    }
}
