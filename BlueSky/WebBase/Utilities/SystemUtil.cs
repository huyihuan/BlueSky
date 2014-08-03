using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using WebBase.SystemClass;

namespace WebBase.Utilities
{
    public class SystemUtil
    {
        public const string SYSTEM_SESSIONKEY_USERNAME = "bluesky_userinfo_username";
        public const string SYSTEM_SESSIONKEY_USERID = "bluesky_userinfo_userid";
        public static void LoginUser(UserInformation _oUser)
        {
            if (null == _oUser)
                return;
            HttpContext.Current.Session[SYSTEM_SESSIONKEY_USERNAME] = _oUser.UserName;
            HttpContext.Current.Session[SYSTEM_SESSIONKEY_USERID] = _oUser.Id;
        }

        public static void LogoutUser()
        {
            HttpContext.Current.Session.Remove(SYSTEM_SESSIONKEY_USERNAME);
            HttpContext.Current.Session.Remove(SYSTEM_SESSIONKEY_USERID);
        }


        public static string GetCurrentUserName()
        {
            return HttpContext.Current.Session[SYSTEM_SESSIONKEY_USERNAME] + "";
        }

        public static int GetCurrentUserId()
        {
            if (null == HttpContext.Current.Session[SYSTEM_SESSIONKEY_USERID])
                return -1;
            return (int)HttpContext.Current.Session[SYSTEM_SESSIONKEY_USERID];
        }

        public static string ApplicationPath
        {
            get
            {
                if ("/" == HttpContext.Current.Request.ApplicationPath)
                    return "/";
                else
                    return HttpContext.Current.Request.ApplicationPath + "/";
            }
        }

        public static string ResolveUrl(string _strUrl)
        {
            if (string.IsNullOrEmpty(_strUrl))
                return _strUrl;

            if (!_strUrl.StartsWith("~"))
                return _strUrl;

            if (1 == _strUrl.Length)
                return SystemUtil.ApplicationPath;

            if (_strUrl.ToCharArray()[1] == '/' || _strUrl.ToCharArray()[1] == '\\')
                return SystemUtil.ApplicationPath + _strUrl.Substring(2);
            else
                return SystemUtil.ApplicationPath + _strUrl.Substring(1);
        }
        
        public static string ResovleModulePath(string _strModuleFullName)
        {
            if (string.IsNullOrEmpty(_strModuleFullName))
                return "";
            _strModuleFullName = _strModuleFullName.Replace("WebWorld.","");
            string[] alParts = _strModuleFullName.Split('.');
            _strModuleFullName = string.Format("{0}\\{1}.View\\", _strModuleFullName.Replace(".", "\\") , alParts[alParts.Length - 1]);
            return _strModuleFullName;
        }

        public static string ResovleControlPath(string _strModuleFullName, string _strControlName)
        {
            if (string.IsNullOrEmpty(_strModuleFullName) || string.IsNullOrEmpty(_strControlName))
                return "";
            return string.Format("{0}\\{1}.ascx", ResovleModulePath(_strModuleFullName), _strControlName);
        }

        public static string ResovleActionFormURL(int _nFunctionId)
        {
            if (_nFunctionId <= 0)
                return "";
            return SystemUtil.ResolveUrl("~/Window.aspx?fn=" + _nFunctionId);
        }

        public static string ResovleActionFormURL(int _nFunctionId, string _strDefaultActionKey)
        {
            string strURL = ResovleActionFormURL(_nFunctionId);
            if(!string.IsNullOrEmpty(_strDefaultActionKey))
                strURL += "&akey=" + _strDefaultActionKey;
            return strURL;
        }

        public static string ResovleActionFormURL(int _nFunctionId, string _strDefaultActionKey, string _strExtraParameters)
        {
            string strURL = SystemUtil.ResovleActionFormURL(_nFunctionId, _strDefaultActionKey);
            if (!string.IsNullOrEmpty(_strExtraParameters))
                strURL += _strExtraParameters.StartsWith("&") ? _strExtraParameters : ("&" + _strExtraParameters);
            return strURL;
        }

        public static string ResovleActionFormURL(System.Web.HttpRequest _hRequest)
        {
            if (null == _hRequest)
                return "";
            return SystemUtil.ResovleActionFormURL(PageUtil.GetQueryInt(_hRequest, "fn", -1));
        }

        public static string ResovleActionFormURL(System.Web.HttpRequest _hRequest, string _strDefaultActionKey)
        {
            if (null == _hRequest)
                return "";
            return SystemUtil.ResovleActionFormURL(PageUtil.GetQueryInt(_hRequest, "fn", -1), _strDefaultActionKey);
        }

        public static string ResovleActionFormURL(System.Web.HttpRequest _hRequest, string _strDefaultActionKey, string _strExtraParameters)
        {
            if (null == _hRequest)
                return "";
            return SystemUtil.ResovleActionFormURL(PageUtil.GetQueryInt(_hRequest, "fn", -1), _strDefaultActionKey, _strExtraParameters);
        }

        public static string ResovleActionFormURL(string _strActionKey)
        { 
            if(string.IsNullOrEmpty(_strActionKey))
                return "";
            SystemAction oAction = SystemAction.Get(_strActionKey);
            if (null == oAction)
                return "";
            return SystemUtil.ResovleActionFormURL(oAction.FunctionId);
        }

        public static string ResovleActionFormURL(string _strActionKey, string _strDefaultActionKey)
        {
            string strURL = SystemUtil.ResovleActionFormURL(_strActionKey);
            if (!string.IsNullOrEmpty(_strDefaultActionKey))
                strURL += "&akey=" + _strDefaultActionKey;
            return strURL;
        }

        public static string ResovleActionFormURL(string _strActionKey, string _strDefaultActionKey, string _strExtraParameters)
        {
            string strURL = SystemUtil.ResovleActionFormURL(_strActionKey, _strDefaultActionKey);
            if (!string.IsNullOrEmpty(_strExtraParameters))
                strURL += _strExtraParameters.StartsWith("&") ? _strExtraParameters : ("&" + _strExtraParameters);
            return strURL;
        }

        public static string ResovleSingleFormUrl(int _nFunctionId, string _strControlName)
        {
            if (_nFunctionId <= 0 || string.IsNullOrEmpty(_strControlName))
                return "";
            return SystemUtil.ResolveUrl(string.Format("~/Window.aspx?fn={0}&fm={1}", _nFunctionId, _strControlName));
        }
        
        public static string ResovleSingleFormUrl(int _nFunctionId, string _strControlName, string _strExtraParameters)
        {
            return ResovleSingleFormUrl(_nFunctionId, _strControlName) + (_strExtraParameters.StartsWith("&") ? _strExtraParameters : ("&" + _strExtraParameters));
        }

        public static string ResovleSingleFormUrl(System.Web.HttpRequest _hRequest, string _strControlName)
        {
            if (null == _hRequest || string.IsNullOrEmpty(_strControlName))
                return "";
            int nFn = PageUtil.GetQueryInt(_hRequest, "fn", -1);
            return SystemUtil.ResovleSingleFormUrl(nFn, _strControlName);
        }

        public static string ResovleSingleFormUrl(System.Web.HttpRequest _hRequest, string _strControlName, string _strExtraParameters)
        {
            if (null == _hRequest || string.IsNullOrEmpty(_strControlName))
                return "";
            int nFn = PageUtil.GetQueryInt(_hRequest, "fn", -1);
            return SystemUtil.ResovleSingleFormUrl(nFn, _strControlName, _strExtraParameters);
        }

        public static string ResovleSingleFormUrl(string _strModuleKey, string _strControlName)
        {
            SystemModule oModule = SystemModule.Get(_strModuleKey);
            if (null == oModule)
                return "";
            return SystemUtil.ResolveUrl(string.Format("~/Window.aspx?m={0}&fm={1}", oModule.Id, _strControlName));
        }

        public static string ResovleSingleFormUrl(string _strModuleKey, string _strControlName, string _strExtraParameters)
        {
            string strURL = ResovleSingleFormUrl(_strModuleKey, _strControlName);
            if(!string.IsNullOrEmpty(_strExtraParameters))
                strURL += _strExtraParameters.StartsWith("&") ? _strExtraParameters : ("&" + _strExtraParameters);
            return strURL;
        }

        public static bool IsFromPermission(System.Web.HttpRequest _hRequest)
        {
            return string.IsNullOrEmpty(_hRequest.QueryString.Get("m"));
        }

        public static string ResovleModuleUploadPath(string _strModuleName)
        {
            return GetVirtualSysUploadPath() + _strModuleName;
        }

        public static void VCodeSaveCurrent(string _strVCode)
        {
            HttpContext.Current.Session["bs_server_vcode"] = _strVCode;
        }

        public static string VCodeGetCurrent()
        {
            return HttpContext.Current.Session["bs_server_vcode"] + "";
        }

        public static string ResovleActionImagePath(string _strImageName)
        {
            return ActionImagePath + _strImageName;
        }

        public static string ActionImagePath
        {
            get
            {
                return SystemUtil.ResolveUrl("~/Include/image/ActionImages/");
            }
        }

        public static string GetVirtualSysUploadPath()
        {
            return SystemUtil.ResolveUrl("SystemUpload\\");
        }

    }
}
