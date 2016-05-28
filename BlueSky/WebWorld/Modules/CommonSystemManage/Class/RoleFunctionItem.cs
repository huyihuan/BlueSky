using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBase;

namespace WebWorld.Modules.CommonSystemManage.Class
{
    public class RoleFunctionItem : DataBase.Interface.IEntity
    {
        public int Id;
        public int RoleItemId;
        public int FunctionItemId;

        public string GetTableName() { return "RoleFunctionItem"; }
        public string GetKeyName() { return "Id"; }

        public static RoleFunctionItem Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            string strFilter = "Id=" + _nId;
            RoleFunctionItem uItem = new RoleFunctionItem();
            RoleFunctionItem[] alist = (RoleFunctionItem[])HEntityCommon.HEntity(uItem).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", uItem.GetTableName(), uItem.GetKeyName(), _nId));
            return alist[0];
        }

        public static RoleFunctionItem[] GetRoleFunctions(int _nRoleItemId)
        {
            if (_nRoleItemId <= 0)
                return null;
            RoleFunctionItem item = new RoleFunctionItem();
            item.RoleItemId = _nRoleItemId;
            RoleFunctionItem[] alItems = (RoleFunctionItem[])HEntityCommon.HEntity(item).EntityList();
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static RoleFunctionItem[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            RoleFunctionItem RoleItem = new RoleFunctionItem();
            RoleFunctionItem[] al = (RoleFunctionItem[])DataBase.HEntityCommon.HEntity(RoleItem).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == al || al.Length == 0)
                return null;
            return al;
        }

        public static RoleFunctionItem[] List()
        {
            RoleFunctionItem[] alItems = (RoleFunctionItem[])DataBase.HEntityCommon.HEntity(new RoleItem()).EntityList();
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static void Delete(int _nId)
        {
            RoleFunctionItem delObj = Get(_nId);
            if (null == delObj)
                return;
            //1、删除该角色Funcion所包含的Action
            RoleActionItem[] alActions = RoleActionItem.GetRoleActions(delObj.RoleItemId, delObj.FunctionItemId);
            int nCount = alActions.Length;
            for (int i = 0; i < nCount; i++)
                RoleActionItem.Delete(alActions[i].Id);

            DataBase.HEntityCommon.HEntity(delObj).EntityDelete();
        }

    }
}
