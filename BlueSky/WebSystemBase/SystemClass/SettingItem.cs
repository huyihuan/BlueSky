using System;
using System.Collections.Generic;
using System.Web;
using DataBase;

namespace WebSystemBase.SystemClass
{
    public class SettingItem
    {
        public int Id;
        public string Key;
        public string Value;

        public string GetTableName() { return "SettingItem"; }
        public string GetKeyName() { return "Id"; }

        public static SettingItem Get(int __nId)
        {
            if (__nId <= 0)
                return null;
            string strFilter = "Id=" + __nId;
            SettingItem oItem = new SettingItem();
            SettingItem[] alist = (SettingItem[])HEntityCommon.HEntity(oItem).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oItem.GetTableName(), oItem.GetKeyName(), __nId));
            return alist[0];
        }

        public static void Delete(int __nId)
        {
            SettingItem delObj = Get(__nId);
            if (null == delObj)
                return;
            DataBase.HEntityCommon.HEntity(delObj).EntityDelete();
        }

        public static SettingItem GetSetting(string __strKey)
        {
            if(string.IsNullOrEmpty(__strKey.Trim()))
                return null;
            string strFilter = string.Format("Key='{0}'", __strKey);
            SettingItem oItem = new SettingItem();
            SettingItem[] alist = (SettingItem[])HEntityCommon.HEntity(oItem).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-Key:{1} exist mutil records", oItem.GetTableName(), __strKey));
            return alist[0];
        }

        public static void SaveSetting(string __strKey, object __oVlue)
        {
            SettingItem oItem = GetSetting(__strKey);
            if (null == oItem)
            {
                oItem = new SettingItem();
                oItem.Key = __strKey;
            }
            oItem.Value = __oVlue + "";
            DataBase.HEntityCommon.HEntity(oItem).EntitySave();
        }

        public static void DeleteSetting(string __strKey)
        { 
            SettingItem oItem = GetSetting(__strKey);
            if (null == oItem)
                return;
            DataBase.HEntityCommon.HEntity(oItem).EntityDelete();
        }   
    }
}
