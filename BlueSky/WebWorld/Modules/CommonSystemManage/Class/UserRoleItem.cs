using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBase;

namespace WebWorld.Modules.CommonSystemManage.Class
{
    public class UserRoleItem : DataBase.Interface.IEntity
    {
        public int Id;
        public int UserItemId;
        public int RoleItemId;

        public string GetTableName() { return "UserRoleItem"; }
        public string GetKeyName() { return "Id"; }

        public string PropertyRoleName
        {
            get
            {
                RoleItem roleObj = RoleItem.Get(this.RoleItemId);
                if (null == roleObj)
                    return "";
                return roleObj.RoleName;
            }
        }

        public static UserRoleItem Get(int __nId)
        {
            if (__nId <= 0)
                return null;
            string strFilter = "Id=" + __nId;
            UserRoleItem uItem = new UserRoleItem();
            UserRoleItem[] alist = (UserRoleItem[])HEntityCommon.HEntity(uItem).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", uItem.GetTableName(), uItem.GetKeyName(), __nId));
            return alist[0];
        }
        
        public static void Delete(int __nId)
        {
            UserRoleItem delObj = Get(__nId);
            if (null == delObj)
                return;
            DataBase.HEntityCommon.HEntity(delObj).EntityDelete();
        }

        public static int Save(UserRoleItem _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }

        public static UserRoleItem[] GetUserRoles(int _nUserItemId)
        {
            string strFilter = "UserItemId=" + _nUserItemId;
            UserRoleItem[] alItems = (UserRoleItem[])DataBase.HEntityCommon.HEntity(new UserRoleItem()).EntityList(strFilter);
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static void DeleteUserRoles(int _nUserItemId)
        {
            UserRoleItem[] alItems = UserRoleItem.GetUserRoles(_nUserItemId);
            if (null == alItems)
                return;
            int nCount = alItems.Length;
            for (int i = 0; i < nCount; i++)
            {
                Delete(alItems[i].Id);
            }
        }

    }
}
