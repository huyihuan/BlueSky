using System;
using System.Collections.Generic;
using System.Web;
using DataBase;
using System.Web.UI.WebControls;

namespace WebSystemBase.SystemClass
{
    public class SystemRole : DataBase.Interface.IEntity
    {
        public int Id;
        public string Name;
        public string Description;

        #region IEntity

        public string GetTableName() { return "SystemRole"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemRole Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemRole oGet = new SystemRole();
            SystemRole[] alist = (SystemRole[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static void Delete(int _nId)
        {
            SystemRole oDel = Get(_nId);
            if (null == oDel)
                return;
            DataBase.HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static bool Exist(string _strName)
        {
            if (string.IsNullOrEmpty(_strName))
                return true;
            SystemRole oExist = new SystemRole();
            string strFilter = string.Format("Name='{0}'", _strName);
            SystemRole[] alist = (SystemRole[])HEntityCommon.HEntity(oExist).EntityList(strFilter);
            if (null == alist)
                return false;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oExist.GetTableName(), "Key", _strName));
            return alist.Length == 1;
        }

        public static SystemRole[] List()
        {
            SystemRole[] alist = (SystemRole[])DataBase.HEntityCommon.HEntity(new SystemRole()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemRole[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemRole oList = new SystemRole();
            SystemRole[] alist = (SystemRole[])DataBase.HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void BindList(ListControl _ltControl, bool _bIncludeSel, bool _bIncludeAll, bool _bIncludeTip)
        {
            if (null == _ltControl)
                return;
            _ltControl.Items.Clear();
            if (_bIncludeAll)
                _ltControl.Items.Add(new ListItem(Constants.ItemAll, ""));
            if (_bIncludeSel)
                _ltControl.Items.Add(new ListItem(Constants.ItemSelect,""));
            SystemRole[] alist = List();
            if (null == alist || alist.Length == 0)
                return;
            foreach (SystemRole item in alist)
            {
                ListItem li = new ListItem(item.Name, item.Id + "");
                if (_bIncludeTip)
                    li.Attributes["title"] = item.Description;
                _ltControl.Items.Add(li);
            }
        }

        public static int Save(SystemRole _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }


    }
}
