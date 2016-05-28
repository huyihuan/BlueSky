using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using DataBase;
using System.Web.UI.WebControls;

namespace WebWorld.Modules.CommonSystemManage.Class
{
    public class FunctionItem : DataBase.Interface.IEntity
    {
        public int Id;
        public string Name;
        public string Value;
        public string Tip;
        public int ParentId;
        public string IconName; 
        public string Description;
        public string Target;
        public int Width;
        public int Height;
        public int IsResize;
        public int IsToMove;
        public int IsIncludeMinBox;
        public int IsIncludeMaxBox;
        public int IsShowInTaskBar;

        public int Level; //功能的深度（默认值：0 即第一级，1,2,3,4,5）

        public string GetTableName() { return "FunctionItem"; }
        public string GetKeyName() { return "Id"; }

        public static FunctionItem Get(int __nId)
        {
            if (__nId <= 0)
                return null;
            string strFilter = "Id=" + __nId;
            FunctionItem funcItem = new FunctionItem();
            FunctionItem[] alist = (FunctionItem[])HEntityCommon.HEntity(funcItem).EntityList(strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", funcItem.GetTableName(), funcItem.GetKeyName(), __nId));
            return alist[0];
        }

        public static void Delete(int __nId)
        {
            FunctionItem delObj = Get(__nId);
            if (null == delObj)
                return;
            FunctionItem[] alSons = GetFunctions(__nId, true);
            int nCount = alSons.Length;
            for (int i = 0; i < nCount; i++)
            {
                DataBase.HEntityCommon.HEntity(alSons[i]).EntityDelete();
            }
        }

        public static FunctionItem[] GetFunctions(int __nParentId, bool __bIncludeAllChildren)
        {
            string strFilter = "ParentId =" + __nParentId;
            if(__bIncludeAllChildren)
                strFilter = "ParentId >=" + __nParentId;
            FunctionItem[] alItems = (FunctionItem[])DataBase.HEntityCommon.HEntity(new FunctionItem()).EntityList(strFilter);
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static FunctionItem[] GetFunctions(int _nParentId, string[] _alRoleItemIds, bool __bIncludeAllChildren)
        {
            string strFilter = "ParentId =" + _nParentId;
            if (__bIncludeAllChildren)
                strFilter = "ParentId >=" + _nParentId;
            strFilter += string.Format(" and Id in (select distinct FunctionItemId from RoleFunctionItem where RoleItemId in ({0}))", String.Join(",", _alRoleItemIds));
            FunctionItem[] alItems = (FunctionItem[])DataBase.HEntityCommon.HEntity(new FunctionItem()).EntityList(strFilter);
            if (null == alItems || alItems.Length == 0)
                return null;
            return alItems;
        }

        public static Hashtable GetParentIdToCount(FunctionItem[] __alFunctions)
        {
            Hashtable htParentIdToCount = new Hashtable();
            int nCount = __alFunctions.Length;
            for (int i = 0; i < nCount; i++)
            {
                int nParentId = __alFunctions[i].ParentId;
                int nChildrenCount = 0;
                for (int j = 0; j < nCount; j++)
                {
                    if (nParentId == __alFunctions[j].ParentId)
                        nChildrenCount++;
                }
                htParentIdToCount[nParentId] = nChildrenCount;
            }
            return htParentIdToCount;
        }
    }
}
