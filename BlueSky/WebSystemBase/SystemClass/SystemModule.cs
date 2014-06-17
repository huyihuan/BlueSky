using System;
using System.Collections.Generic;
using System.Text;

using BlueSky.Interfaces;
using BlueSky.EntityAccess;

namespace WebSystemBase.SystemClass
{
    public class SystemModule : IEntity
    {
        public int Id;
        public string Name;
        public string Key;
        public string Controller;
        public string Description;
        public string IconName;
        public int OrderId;

        #region IEntity

        public string GetTableName() { return "SystemModule"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        public static SystemModule Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemModule oGet = new SystemModule();
            SystemModule[] alist = (SystemModule[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemModule Get(string _strKey)
        {
            if (string.IsNullOrEmpty(_strKey))
                return null;
            SystemModule oGet = new SystemModule();
            SystemModule[] alist = (SystemModule[])HEntityCommon.HEntity(oGet).EntityList(string.Format("[Key]='{0}'", _strKey));
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), "Key", _strKey));
            return alist[0];
        }

        public static void Delete(int _nId)
        {
            SystemModule oDel = Get(_nId);
            if (null == oDel)
                return;
            //1、删除角色模块关联表
            SystemRoleModulePermission[] alRoleModule = SystemRoleModulePermission.GetByModuleId(_nId);
            if (null != alRoleModule && alRoleModule.Length > 0)
            {
                foreach (SystemRoleModulePermission permission in alRoleModule)
                    SystemRoleModulePermission.Delete(permission.Id);
            }

            //2、删除用户模块关联表
            SystemUserModulePermission[] alUserModule = SystemUserModulePermission.GetByModuleId(_nId);
            if (null != alUserModule && alUserModule.Length > 0)
            {
                foreach (SystemUserModulePermission permission in alUserModule)
                    SystemUserModulePermission.Delete(permission.Id);
            }

            //3、删除模块功能
            SystemFunction rootFunction = SystemFunction.Get(oDel.Key);
            if (null != rootFunction)
                SystemFunction.Delete(rootFunction.Id);

            //3、删除模块
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static bool Exist(string _strKey)
        {
            if (string.IsNullOrEmpty(_strKey))
                return true;
            SystemModule oExist = new SystemModule();
            string strFilter = string.Format("[Key]='{0}'", _strKey);
            int nExistCount = HEntityCommon.HEntity(oExist).EntityCount(strFilter);
            if (nExistCount > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oExist.GetTableName(), "Key", _strKey));
            return nExistCount == 1;
        }

        public static SystemModule[] List()
        {
            SystemModule[] alist = (SystemModule[])HEntityCommon.HEntity(new SystemModule()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemModule[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemModule oList = new SystemModule();
            SystemModule[] alist = (SystemModule[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static int Save(SystemModule _saveObj)
        {
            if (null == _saveObj)
                return -1;
            int nModuleId = HEntityCommon.HEntity(_saveObj).EntitySave();
            if (nModuleId <= 0)
                return -1;

            //1、新增功能根节点
            SystemFunction rootFn = SystemFunction.Get(_saveObj.Key);
            if (null == rootFn)
            {
                rootFn = new SystemFunction();
                rootFn.Key = _saveObj.Key;
                rootFn.ModuleId = nModuleId;
                rootFn.Level = 1;
                rootFn.ParentId = -1;
            }
            rootFn.Name = _saveObj.Name;
            rootFn.IconName = _saveObj.IconName;
            rootFn.Description = _saveObj.Description;
            rootFn.OrderId = _saveObj.OrderId;
            SystemFunction.Save(rootFn);

            return nModuleId;
        }
    }
}
