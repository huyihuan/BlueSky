using System;
using System.Collections.Generic;
using System.Text;
using BlueSky.Interfaces;
using BlueSky.EntityAccess;

namespace WebSystemBase.SystemClass
{
    public class SystemRoleModulePermission : IEntity
    {

        public int Id;
        public int RoleId;
        public string ModuleId;

        #region IEntity

        public string GetTableName() { return "SystemRoleModulePermission"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemRoleModulePermission Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemRoleModulePermission oGet = new SystemRoleModulePermission();
            SystemRoleModulePermission[] alist = (SystemRoleModulePermission[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemRoleModulePermission[] GetByModuleId(int _nModuleId)
        {
            if (_nModuleId <= 0)
                return null;
            SystemRoleModulePermission oGet = new SystemRoleModulePermission();
            SystemRoleModulePermission[] alist = (SystemRoleModulePermission[])HEntityCommon.HEntity(oGet).EntityList("ModuleId=" + _nModuleId);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void Delete(int _nId)
        {
            SystemRoleModulePermission oDel = Get(_nId);
            if (null == oDel)
                return;
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static SystemRoleModulePermission[] List()
        {
            SystemRoleModulePermission[] alist = (SystemRoleModulePermission[])HEntityCommon.HEntity(new SystemRoleModulePermission()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRoleModulePermission[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemRoleModulePermission oList = new SystemRoleModulePermission();
            SystemRoleModulePermission[] alist = (SystemRoleModulePermission[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }
    }
}
