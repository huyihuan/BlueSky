using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBase;

namespace WebWorld.Modules.CommonSystemManage.Class
{
    public class RoleActionItem : DataBase.Interface.IEntity
    {
        public int Id;
        public int RoleItemId;
        public int FunctionItemId;
        public int ActionItemId;

        public string GetTableName() { return "RoleActionItem"; }
        public string GetKeyName() { return "Id"; }

        public static RoleActionItem Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            string strFilter = "Id=" + _nId;
            RoleActionItem oItem = new RoleActionItem();
            RoleActionItem[] alist = (RoleActionItem[])HEntityCommon.HEntity(oItem).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oItem.GetTableName(), oItem.GetKeyName(), _nId));
            return alist[0];
        }

        public static RoleActionItem[] GetRoleActions(int _nRoleItemId)
        {
            if (_nRoleItemId <= 0)
                return null;
            RoleActionItem item = new RoleActionItem();
            item.RoleItemId = _nRoleItemId;
            RoleActionItem[] alItems = (RoleActionItem[])HEntityCommon.HEntity(item).EntityList();
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static RoleActionItem[] GetRoleActions(int _nRoleItemId, int _nFunctionItemId)
        {
            if (_nRoleItemId <= 0 || _nFunctionItemId <= 0)
                return null;
            RoleActionItem item = new RoleActionItem();
            item.RoleItemId = _nRoleItemId;
            item.FunctionItemId = _nFunctionItemId;
            RoleActionItem[] alItems = (RoleActionItem[])HEntityCommon.HEntity(item).EntityList();
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static RoleActionItem[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            RoleActionItem RoleItem = new RoleActionItem();
            RoleActionItem[] al = (RoleActionItem[])DataBase.HEntityCommon.HEntity(RoleItem).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == al || al.Length == 0)
                return null;
            return al;
        }

        public static RoleActionItem[] List()
        {
            RoleActionItem[] alItems = (RoleActionItem[])DataBase.HEntityCommon.HEntity(new RoleItem()).EntityList();
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static void Delete(int _nId)
        {
            RoleActionItem delObj = Get(_nId);
            if (null == delObj)
                return;
            DataBase.HEntityCommon.HEntity(delObj).EntityDelete();
        }
    }
}
