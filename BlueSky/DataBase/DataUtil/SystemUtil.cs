using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace DataBase
{
    public class SystemUtil
    {
        public static void SaveLoginUser(Hashtable _htUserInformation)
        {
            HttpContext.Current.Session["bs_login_username"] = _htUserInformation["UserName"] + "";
            HttpContext.Current.Session["bs_login_userid"] = (int)_htUserInformation["Id"];
        }

        public static string GetCurrentUserName()
        {
            return HttpContext.Current.Session["bs_login_username"] + "";
        }

        public static int GetCurrentUserId()
        {
            if (null == HttpContext.Current.Session["bs_login_userid"])
                return -1;
            return (int)HttpContext.Current.Session["bs_login_userid"];
        }

        public static string ResovleModulePath(string _strModuleFullName)
        {
            if (string.IsNullOrEmpty(_strModuleFullName))
                return "";
            _strModuleFullName = _strModuleFullName.Replace("WebWorld.","");
            //_strModuleFullName = _strModuleFullName.Replace("Modules.", "");
            _strModuleFullName = _strModuleFullName.Replace(".", "\\");
            return _strModuleFullName;
        }

        public static string ResovleFunctionUrl(string _strModuleFullName, string _strFunctionKey)
        {
            if (string.IsNullOrEmpty(_strModuleFullName) || string.IsNullOrEmpty(_strFunctionKey))
                return "";
            return string.Format("{0}\\{1}.ascx", ResovleModulePath(_strModuleFullName), _strFunctionKey);
        }

    }
}
