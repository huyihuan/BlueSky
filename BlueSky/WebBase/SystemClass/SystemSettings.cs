using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSky.Interfaces;
using BlueSky.EntityAccess;
using BlueSky.Attribute;

namespace WebSystemBase.SystemClass
{
    public class SystemSetting : IEntity
    {
        [EntityFieldAttribute(FieldName = "Id", IsPrimaryKey = true)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
        public const string KEY_ENABLEAUTOLOGIN = "KEY_ENABLEAUTOLOGIN";
        public const string KEY_AUTOLOGINUSERNAME = "KEY_AUTOLOGINUSERNAME";
        public const string KEY_AUTOLOGINPASSWORD = "KEY_AUTOLOGINPASSWORD";
        public static SystemSetting Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            return EntityAccess<SystemSetting>.Access.Get(_nId);
        }
        public static void Delete(int _nId)
        {
            SystemSetting oDel = Get(_nId);
            if (null == oDel)
                return;
            SystemSetting.Delete(oDel);
        }
        public static void Delete(SystemSetting _oDel)
        {
            if (null == _oDel)
                return;
            EntityAccess<SystemSetting>.Access.Delete(_oDel);
        }
        public static void DeleteUserSetting(int _nUserId)
        {
            if (_nUserId <= 0)
                return;
            SystemSetting oGet = new SystemSetting();
            oGet.UserId = _nUserId;
            SystemSetting[] alist = EntityAccess<SystemSetting>.Access.List(oGet);
            if (null != alist && alist.Length >= 1)
            { 
                foreach(SystemSetting oSetting in alist)
                    SystemSetting.Delete(oSetting);
            }
        }
        public static int Save(SystemSetting _oSave)
        {
            if (null == _oSave)
                return -1;
            return EntityAccess<SystemSetting>.Access.Save(_oSave);
        }
        public static SystemSetting GetSetting(string _strKey)
        {
            return SystemSetting.GetSetting(_strKey, -1);
        }
        public static void SetSetting(string _strKey, object _oValue)
        {
            SystemSetting.SetSetting(_strKey, _oValue, -1);
        }
        public static SystemSetting GetSetting(string _strKey, int _nUserId)
        {
            if (string.IsNullOrEmpty(_strKey))
                return null;
            string strFilter = string.Format("Key='{0}'", _strKey);
            if (_nUserId > 0)
            {
                strFilter += (" and UserId=" + _nUserId);
            }
            else
            {
                strFilter += " and (UserId is null or UserId=0)";
            }
            SystemSetting[] alist = EntityAccess<SystemSetting>.Access.List(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            return alist[0];
        }
        public static void SetSetting(string _strKey,object _oValue, int _nUserId)
        {
            if (string.IsNullOrEmpty(_strKey))
                return;
            SystemSetting oSave = GetSetting(_strKey, _nUserId);
            if (null == oSave)
            {
                oSave = new SystemSetting();
                oSave.Key = _strKey;
            }
            oSave.Value = _oValue == null ? null : _oValue.ToString();
            if (_nUserId > 0)
            {
                oSave.UserId = _nUserId;
            }
            SystemSetting.Save(oSave);
        }
    }
}
