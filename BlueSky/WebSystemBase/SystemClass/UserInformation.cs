using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;

using BlueSky.Interfaces;
using System.Web.UI.WebControls;
using BlueSky.EntityAccess;
using BlueSky.Utilities;

namespace WebSystemBase.SystemClass
{
    public class UserInformation : IEntity
    {
        public int Id;
		public string UserName;
		public string Password;
		public string NickName;
		public int Gender;
        public string CardID;
        public int Age;
        public string Email;
        public string PostCode;
        public string MSN;
        public string Country;
        public string QQ;

        public const string CONST_STR_DEFAULTPASSWORD = "123456";    //新增用户初始密码

        #region IEntity

        public string GetTableName() { return "UserInformation"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public string PropertyRoleName
        {
            get
            {
                SystemUserRole[] alist = SystemUserRole.GetUserRoles(this.Id);
                if (null == alist)
                    return "";
                string strRoleName = "";
                foreach (SystemUserRole item in alist)
                {
                    if ("" != strRoleName)
                        strRoleName += ",";
                    strRoleName += item.PropertyRoleName;
                }
                return strRoleName;
            }
        }

        public string PropertyGender
        {
            get { return Gender == 1 ? "男" : "女"; }
        }

        public static UserInformation Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            UserInformation oGet = new UserInformation();
            return (UserInformation)HEntityCommon.HEntity(oGet).EntityGet(_nId);
        }

        public static UserInformation Get(string _strUserName)
        {
            UserInformation oGet = new UserInformation();
            oGet.UserName = _strUserName;
            UserInformation[] alist = (UserInformation[])HEntityCommon.HEntity(oGet).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}(UserName:{1}) exist mutil records！", oGet.GetTableName(), _strUserName));
            return alist[0];
        }

        public static UserInformation[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            UserInformation oList = new UserInformation();
            UserInformation[] alist = (UserInformation[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void Delete(int _nId)
        {
            UserInformation oDel = Get(_nId);
            if (null == oDel)
                return;
            //1、删除用户权限
            SystemUserModulePermission[] alModulePermission = SystemUserModulePermission.GetUserModulePermission(_nId);
            if (null != alModulePermission && alModulePermission.Length != 0)
            {
                foreach (SystemUserModulePermission oModulePermission in alModulePermission)
                {
                    SystemUserFunctionPermission[] alFuncitnPermission = SystemUserFunctionPermission.GetUserFunctionPermission(_nId, oModulePermission.ModuleId);
                    if (null == alFuncitnPermission || alFuncitnPermission.Length == 0)
                        continue;
                    foreach (SystemUserFunctionPermission oFunctionPermission in alFuncitnPermission)
                    {
                        SystemUserActionPermission[] alActionPermission = SystemUserActionPermission.Get(_nId, oFunctionPermission.FunctionId);
                        if (null == alFuncitnPermission || alFuncitnPermission.Length == 0)
                            continue;
                        foreach (SystemUserActionPermission oActionPermission in alActionPermission)
                            SystemUserActionPermission.Delete(oActionPermission.Id);

                        SystemUserFunctionPermission.Delete(oFunctionPermission.Id);
                    }

                    SystemUserModulePermission.Delete(oModulePermission.Id);
                }
            }

            //2、删除用户角色
            SystemUserRole[] alRole = SystemUserRole.GetUserRoles(_nId);
            if (null != alRole && alRole.Length != 0)
            {
                foreach (SystemUserRole oRole in alRole)
                    SystemUserRole.Delete(oRole.Id);
            }

            //3、删除用户
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static int Save(UserInformation _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }

        public static bool ExistUser(string _strUserName)
        {
            if (string.IsNullOrEmpty(_strUserName))
                return false;
            UserInformation oExist = new UserInformation();
            oExist.UserName = _strUserName;
            int nCount = HEntityCommon.HEntity(oExist).EntityCount();
            if (nCount > 1)
                throw new Exception(string.Format("{0}(UserName:{1}) exist mutil records！",oExist.GetTableName(), _strUserName));
            return nCount == 1;
        }

        public static bool ExistUser(string _strUserName, string _strPassword)
        {
            if (string.IsNullOrEmpty(_strUserName) || string.IsNullOrEmpty(_strPassword))
                return false;
            UserInformation oExist = new UserInformation();
            oExist.UserName = _strUserName;
            oExist.Password = CryptUtil.MD5Encrypt(_strPassword);
            int nCount = HEntityCommon.HEntity(oExist).EntityCount();
            if (nCount > 1)
                throw new Exception(string.Format("{0}(UserName:{1}) exist mutil records！", oExist, _strUserName));
            return nCount == 1;
        }

    }
}
