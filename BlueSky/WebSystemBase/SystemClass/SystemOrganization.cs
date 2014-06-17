using System;
using System.Collections.Generic;
using System.Text;

using BlueSky.Interfaces;
using System.Web.UI.WebControls;
using BlueSky.EntityAccess;
using BlueSky.Utilities;

namespace WebSystemBase.SystemClass
{
    public class SystemOrganization : IEntity
    {
        public int Id;
	    public int ParentId;
	    public string Name;
	    public int TypeId;
        public string Remark;

        #region IEntity

        public string GetTableName() { return "SystemOrganization"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        #region Properties

        public string OrganizationTypeName
        {
            get
            {
                SystemOrganizationType oType = SystemOrganizationType.Get(this.TypeId);
                if (null == oType)
                    return "";
                return oType.Name;
            }
        }

        #endregion

        public static SystemOrganization Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemOrganization oGet = new SystemOrganization();
            SystemOrganization[] alist = (SystemOrganization[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static void Delete(int _nId)
        {
            SystemOrganization oDel = Get(_nId);
            if (null == oDel)
                return;
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static bool Exist(string _strName)
        {
            if (string.IsNullOrEmpty(_strName))
                return true;
            SystemOrganization oExist = new SystemOrganization();
            string strFilter = string.Format("Name='{0}'", _strName);
            SystemOrganization[] alist = (SystemOrganization[])HEntityCommon.HEntity(oExist).EntityList(strFilter);
            if (null == alist)
                return false;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oExist.GetTableName(), "Key", _strName));
            return alist.Length == 1;
        }

        public static SystemOrganization[] List()
        {
            SystemOrganization[] alist = (SystemOrganization[])HEntityCommon.HEntity(new SystemOrganization()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemOrganization[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemOrganization oList = new SystemOrganization();
            SystemOrganization[] alist = (SystemOrganization[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static void BindList(ListControl _ltControl, bool _bIncludeSel, bool _bIncludeAll)
        {
            if (null == _ltControl)
                return;
            _ltControl.Items.Clear();
            if (_bIncludeAll)
                _ltControl.Items.Add(new ListItem(Constants.ItemAll, ""));
            if (_bIncludeSel)
                _ltControl.Items.Add(new ListItem(Constants.ItemSelect, ""));
            SystemOrganization[] alist = List();
            if (null == alist || alist.Length == 0)
                return;
            foreach (SystemOrganization item in alist)
            {
                ListItem li = new ListItem(item.Name, item.Id + "");
                _ltControl.Items.Add(li);
            }
        }

        public static int Save(SystemOrganization _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }

    }
}
