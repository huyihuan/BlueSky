using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using DataBase;
using System.Web.UI.WebControls;

namespace WebWorld.Modules.CommonSystemManage.Class
{
    public class UserItem : DataBase.Interface.IEntity
    {
        public int Id;
		public string UserName;
		public string Password;
		public string NickName;
		public string Gender;
		public string IDCard;
		public string Tel;
		public string Address;
		public string Email;
		public string Post;
		public string QQ;
		public string MSN;

        public string GetTableName() { return "UserItem"; }
        public string GetKeyName() { return "Id"; }

        public string PropertyRoleName
        {
            get
            {
                UserRoleItem[] alItems = UserRoleItem.GetUserRoles(this.Id);
                if (null == alItems)
                    return "";
                string strRoleName = "";
                foreach (UserRoleItem item in alItems)
                {
                    if ("" != strRoleName)
                        strRoleName += ",";
                    strRoleName += item.PropertyRoleName;
                }
                return strRoleName;
            }
        }

        public static UserItem Get(int __nId)
        {
            if (__nId <= 0)
                return null;
            string strFilter = "Id=" + __nId;
            UserItem uItem = new UserItem();
            UserItem[] alist = (UserItem[])HEntityCommon.HEntity(uItem).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", uItem.GetTableName(), uItem.GetKeyName(), __nId));
            return alist[0];
        }

        public static UserItem Get(string _strUserName)
        {
            UserItem item = new UserItem();
            item.UserName = _strUserName;
            UserItem[] alItems = (UserItem[])HEntityCommon.HEntity(item).EntityList();
            if(null == alItems || alItems.Length == 0)
                return null;
            if (alItems.Length > 1)
                throw new Exception(string.Format("UserItem(UserName:{0}) exist mutil records！", _strUserName));
            return alItems[0];
        }

        public static UserItem[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            UserItem userItem = new UserItem();
            UserItem[] al = (UserItem[])DataBase.HEntityCommon.HEntity(userItem).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == al || al.Length == 0)
                return null;
            return al;
        }

        public static void Delete(int __nId)
        {
            UserItem delObj = Get(__nId);
            if (null == delObj)
                return;
			DataBase.HEntityCommon.HEntity(delObj).EntityDelete();
        }

        public static int Save(UserItem _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }

        public static bool ExistUser(string _strUserName)
        {
            if (string.IsNullOrEmpty(_strUserName))
                return false;
            UserItem item = new UserItem();
            item.UserName = _strUserName;
            int nCount = HEntityCommon.HEntity(item).EntityCount();
            if (nCount > 1)
                throw new Exception(string.Format("UserItem(UserName:{0}) exist mutil records！", _strUserName));
            return nCount == 1;
        }

        public static bool ExistUser(string _strUserName, string _strPassword)
        {
            if (string.IsNullOrEmpty(_strUserName) || string.IsNullOrEmpty(_strPassword))
                return false;
            UserItem item = new UserItem();
            item.UserName = _strUserName;
            item.Password = Util.MD5Encrypt(_strPassword);
            int nCount = HEntityCommon.HEntity(item).EntityCount();
            if (nCount > 1)
                throw new Exception(string.Format("UserItem(UserName:{0}) exist mutil records！", _strUserName));
            return nCount == 1;
        }

    }
}
