using System;
using System.Collections.Generic;
using System.Text;

using BlueSky.Interfaces;
using BlueSky.EntityAccess;

namespace WebSystemBase.SystemClass
{
    public class SystemUserModulePermission : IEntity
    {

        public int Id;
        public int UserId;
        public int ModuleId;

        #region IEntity

        public string GetTableName() { return "SystemUserModulePermission"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemUserModulePermission Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemUserModulePermission oGet = new SystemUserModulePermission();
            SystemUserModulePermission[] alist = (SystemUserModulePermission[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemUserModulePermission[] GetByModuleId(int _nModuleId)
        {
            if (_nModuleId <= 0)
                return null;
            SystemUserModulePermission oGet = new SystemUserModulePermission();
            SystemUserModulePermission[] alist = (SystemUserModulePermission[])HEntityCommon.HEntity(oGet).EntityList("ModuleId=" + _nModuleId);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemUserModulePermission[] GetUserModulePermission(int _nUserId)
        {
            if (_nUserId <= 0)
                return null;
            SystemUserModulePermission oGet = new SystemUserModulePermission();
            SystemUserModulePermission[] alist = (SystemUserModulePermission[])HEntityCommon.HEntity(oGet).EntityList("UserId=" + _nUserId);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void Delete(int _nId)
        {
            SystemUserModulePermission oDel = Get(_nId);
            if (null == oDel)
                return;
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static SystemUserModulePermission[] List()
        {
            SystemUserModulePermission[] alist = (SystemUserModulePermission[])HEntityCommon.HEntity(new SystemUserModulePermission()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemUserModulePermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemUserModulePermission oList = new SystemUserModulePermission();
            SystemUserModulePermission[] alist = (SystemUserModulePermission[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }
    }
}
