using System;
using System.Collections.Generic;
using System.Web;

using BlueSky.Interfaces;
using BlueSky.EntityAccess;

namespace WebSystemBase.SystemClass
{
    public class SystemUserRole : IEntity
    {
        public int Id;
        public int UserId;
        public int RoleId;

        #region IEntity

        public string GetTableName() { return "SystemUserRole"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public string PropertyRoleName
        {
            get
            {
                SystemRole oRole = SystemRole.Get(this.RoleId);
                return null == oRole ? "" : oRole.Name;
            }
        }

        public static SystemUserRole Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemUserRole oGet = new SystemUserRole();
            SystemUserRole[] alist = (SystemUserRole[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }
        
        public static void Delete(int _nId)
        {
            SystemUserRole oDel = Get(_nId);
            if (null == oDel)
                return;
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static int Save(SystemUserRole _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }

        public static SystemUserRole[] GetUserRoles(int _nUserId)
        {
            string strFilter = "UserId=" + _nUserId;
            SystemUserRole[] alist = (SystemUserRole[])HEntityCommon.HEntity(new SystemUserRole()).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void DeleteUserRoles(int _nUserId)
        {
            SystemUserRole[] alist = SystemUserRole.GetUserRoles(_nUserId);
            if (null == alist)
                return;
            int nCount = alist.Length;
            for (int i = 0; i < nCount; i++)
            {
                Delete(alist[i].Id);
            }
        }

    }
}
