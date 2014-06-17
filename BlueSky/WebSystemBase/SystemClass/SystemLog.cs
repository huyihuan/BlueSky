using System;
using System.Collections.Generic;
using System.Text;

using BlueSky.Interfaces;
using BlueSky.EntityAccess;
using BlueSky.Utilities;

namespace WebSystemBase.SystemClass
{
    public class SystemLog : IEntity
    {
        public int Id;
        public int UserId;
        public DateTime AccessTime;
        public string AccessFunctionName;
        public string AccessActionName;
        public string AccessURL;
        public string Remark;

        #region IEntity

        public string GetTableName() { return "SystemLog"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        #region Properties

        public string UserName
        {
            get
            {
                UserInformation oItem = UserInformation.Get(this.UserId);
                if (null == oItem)
                    return "";
                return oItem.UserName;
            }
        }

        public string FormattingAccessTime
        {
            get 
            {
                return this.AccessTime.ToString(Constants.DateTimeFormatStandard);    
            }
        }

        #endregion

        public static SystemLog Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemLog oGet = new SystemLog();
            SystemLog[] alist = (SystemLog[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static void Delete(int _nId)
        {
            SystemLog oDel = Get(_nId);
            if (null == oDel)
                return;
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static SystemLog[] List()
        {
            return List("");
        }

        public static SystemLog[] List(string _strFilter)
        {
            SystemLog[] alist = (SystemLog[])HEntityCommon.HEntity(new SystemLog()).EntityList(_strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemLog[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemLog oList = new SystemLog();
            SystemLog[] alist = (SystemLog[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static int Save(SystemLog _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }

    }
}
