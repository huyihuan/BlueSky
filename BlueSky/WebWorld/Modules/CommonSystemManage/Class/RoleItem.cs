using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBase;
using System.Web.UI.WebControls;

namespace WebWorld.Modules.CommonSystemManage.Class
{
    public class RoleItem : DataBase.Interface.IEntity
    {
        public int Id;
        public string RoleName;
        public string Remark;

        public string GetTableName() { return "RoleItem"; }
        public string GetKeyName() { return "Id"; }

        public static RoleItem Get(int __nId)
        {
            if (__nId <= 0)
                return null;
            string strFilter = "Id=" + __nId;
            RoleItem uItem = new RoleItem();
            RoleItem[] alist = (RoleItem[])HEntityCommon.HEntity(uItem).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", uItem.GetTableName(), uItem.GetKeyName(), __nId));
            return alist[0];
        }

        public static RoleItem[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            RoleItem RoleItem = new RoleItem();
            RoleItem[] al = (RoleItem[])DataBase.HEntityCommon.HEntity(RoleItem).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == al || al.Length == 0)
                return null;
            return al;
        }

        public static RoleItem[] List()
        {
            RoleItem[] alItems = (RoleItem[])DataBase.HEntityCommon.HEntity(new RoleItem()).EntityList();
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static void Delete(int __nId)
        {
            RoleItem delObj = Get(__nId);
            if (null == delObj)
                return;
            DataBase.HEntityCommon.HEntity(delObj).EntityDelete();
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
            RoleItem[] alItems = List();
            if (null == alItems || alItems.Length == 0)
                return;
            foreach (RoleItem item in alItems)
            {
                ListItem li = new ListItem(item.RoleName, item.Id + "");
                if (_bIncludeTip)
                    li.Attributes["title"] = item.Remark;
                _ltControl.Items.Add(li);
            }
        }

    }
}
