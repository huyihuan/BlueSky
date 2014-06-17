using System;
using System.Collections.Generic;
using System.Text;

using BlueSky.Interfaces;
using System.Web.UI.WebControls;
using BlueSky.EntityAccess;
using BlueSky.Utilities;

namespace WebSystemBase.SystemClass
{
    public class SystemOrganizationType : IEntity
    {
        public int Id;
        public int ParentId;
        public string Name;
        public string Remark;

        #region IEntity

        public string GetTableName() { return "SystemOrganizationType"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemOrganizationType Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemOrganizationType oGet = new SystemOrganizationType();
            SystemOrganizationType[] alist = (SystemOrganizationType[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static void Delete(int _nId)
        {
            SystemOrganizationType oDel = Get(_nId);
            if (null == oDel)
                return;
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static bool Exist(string _strName)
        {
            if (string.IsNullOrEmpty(_strName))
                return true;
            SystemOrganizationType oExist = new SystemOrganizationType();
            string strFilter = string.Format("Name='{0}'", _strName);
            SystemOrganizationType[] alist = (SystemOrganizationType[])HEntityCommon.HEntity(oExist).EntityList(strFilter);
            if (null == alist)
                return false;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oExist.GetTableName(), "Key", _strName));
            return alist.Length == 1;
        }

        public static SystemOrganizationType[] List()
        {
            SystemOrganizationType[] alist = (SystemOrganizationType[])HEntityCommon.HEntity(new SystemOrganizationType()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemOrganizationType[] List(string _strFilter)
        {
            SystemOrganizationType[] alist = (SystemOrganizationType[])HEntityCommon.HEntity(new SystemOrganizationType()).EntityList(_strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemOrganizationType[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemOrganizationType oList = new SystemOrganizationType();
            SystemOrganizationType[] alist = (SystemOrganizationType[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        private static void _BindList(ListControl _ltControl,bool _bClearItems, bool _bIncludeSel, bool _bIncludeAll,int _nParentId, bool _bSelectAll,int _nLevel, ListItem[] _alPrependItems)
        {
            if (null == _ltControl)
                return;
            if(_bClearItems)
                _ltControl.Items.Clear();
            if (_bIncludeAll)
                _ltControl.Items.Add(new ListItem(Constants.ItemAll, ""));
            if (_bIncludeSel)
                _ltControl.Items.Add(new ListItem(Constants.ItemSelect, ""));
            if (_nLevel == 0 && _alPrependItems != null && _alPrependItems.Length >= 1)
                _ltControl.Items.AddRange(_alPrependItems);

            SystemOrganizationType[] alist = List("ParentId=" + _nParentId);
            if (null == alist || alist.Length == 0)
                return;
            foreach (SystemOrganizationType item in alist)
            {
                ListItem li = new ListItem("".PadLeft(_nLevel * 3, '.') + item.Name, item.Id + "");
                _ltControl.Items.Add(li);
                if(_bSelectAll)
                    _BindList(_ltControl, false, false, false, item.Id, true, _nLevel + 1, null);

            }
        }

        public static void BindList(ListControl _ltControl, bool _bClearItems, bool _bIncludeSel, bool _bIncludeAll, int _nParentId, bool _bSelectAll)
        {
            _BindList(_ltControl, _bClearItems, _bIncludeSel, _bIncludeAll, _nParentId, _bSelectAll, 0, null);
        }

        public static void BindList(ListControl _ltControl, bool _bClearItems, bool _bIncludeSel, bool _bIncludeAll, int _nParentId, bool _bSelectAll, ListItem[] _alPrependItems)
        {
            _BindList(_ltControl, _bClearItems, _bIncludeSel, _bIncludeAll, _nParentId, _bSelectAll, 0, _alPrependItems);
        }

        public static int Save(SystemOrganizationType _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }
    }
}
