using System;
using System.Collections.Generic;
using System.Web;
using BlueSky.Interfaces;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using WebSystemBase.Utilities;
using BlueSky.Utilities;
using BlueSky.EntityAccess;

namespace WebSystemBase.SystemClass
{
    public class SystemAction : IEntity, IComparer<SystemAction>
    {
	    public int Id;
	    public string Name;
	    public string Key;
        public int IsDefault; //功能默认Action, 1 代表默认
        public int FunctionId;
	    public string ActionType;
	    public string ActionValue;
	    public string Description;
	    public string ControlName;
	    public string Tip;
	    public string IconName;
	    public string Target;
	    public int IsPopup;
	    public int Width;
	    public int Height;
	    public int IsResize;
	    public int IsMove;
	    public int IsIncludeMinBox;
	    public int IsIncludeMaxBox;
	    public int IsShowInTaskBar;
        public int OrderId;
        public int EntityCount; //操作的实体数量，-1代表可操作无数个,0代表无要求，1,2,3..n代表n个

        #region ActionType

        public const string CONST_ACTIONTYPE_NORMAL = "normal";
        public const string CONST_ACTIONTYPE_JAVASCRIPT = "javascript";
        public const string CONST_ACTIONTYPE_HTTP = "http";
        public static Hashtable htActionTypeToName = new Hashtable(new Dictionary<string, string>() { { CONST_ACTIONTYPE_NORMAL, "normal" }, { CONST_ACTIONTYPE_JAVASCRIPT, "javascript" }, { CONST_ACTIONTYPE_HTTP, "http" } });

        public static void BindActionTypeList(ListControl _ltControl, bool _bIncludeSelect, bool _bIncludeAll)
        {
            if (null == _ltControl)
                return;
            _ltControl.Items.Clear();
            if (_bIncludeSelect)
                _ltControl.Items.Add(new ListItem(Constants.ItemSelect, ""));
            if (_bIncludeAll)
                _ltControl.Items.Add(new ListItem(Constants.ItemAll, ""));

            foreach (string strKey in htActionTypeToName.Keys)
            {
                ListItem li = new ListItem();
                li.Text =  htActionTypeToName[strKey] + "";
                li.Value = strKey;
                _ltControl.Items.Add(li);
            }
        }

        #endregion

        #region IEntity

        public string GetTableName() { return "SystemAction"; }
        public string GetKeyName() { return "Id"; }

        #endregion

        #region IComparer

        public int Compare(SystemAction x,SystemAction y)
        {
            return x.OrderId - y.OrderId;
        }

        #endregion

        #region List.Contains
        //用于List<SystemAction>类型的Contains方法比较引用类型
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            return this.Key.Equals(((SystemAction)obj).Key);
        }

        #endregion

        public string ToActionJson()
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("{");
            sbJson.Append(string.Format("actionType:'{0}',",this.ActionType));
            sbJson.Append(string.Format("actionKey:'{0}',", this.Key));
            sbJson.Append(string.Format("entityCount:{0},", this.EntityCount));
            sbJson.Append(string.Format("fn:{0},", this.FunctionId));
            sbJson.Append(string.Format("popup:{0},", (this.IsPopup).ToString().ToLower()));
            sbJson.Append(string.Format("width:{0},", this.Width));
            sbJson.Append(string.Format("height:{0},", this.Height));
            sbJson.Append(string.Format("title:'{0}',", this.Name));
            sbJson.Append(string.Format("resize:{0},", (this.IsResize == Constants.Yes).ToString().ToLower()));
            sbJson.Append(string.Format("maxbox:{0},", (this.IsIncludeMaxBox == Constants.Yes).ToString().ToLower()));
            sbJson.Append(string.Format("minbox:{0},", (this.IsIncludeMinBox == Constants.Yes).ToString().ToLower()));
            sbJson.Append(string.Format("move:{0},", (this.IsMove == Constants.Yes).ToString().ToLower()));
            sbJson.Append(string.Format("actionValue:'{0}',", this.ActionValue));
            sbJson.Append(string.Format("iconURL:'{0}'", SystemUtil.ResovleActionImagePath(this.IconName)));
            sbJson.Append("}");
            return sbJson.ToString();
        }

        public static SystemAction Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            SystemAction oGet = new SystemAction();
            SystemAction[] alist = (SystemAction[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static SystemAction Get(string _strKey)
        {
            if (string.IsNullOrEmpty(_strKey))
                return null;
            SystemAction oGet = new SystemAction();
            SystemAction[] alist = (SystemAction[])HEntityCommon.HEntity(oGet).EntityList(string.Format("[Key]='{0}'", _strKey));
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), "Key", _strKey));
            return alist[0];
        }

        public static SystemAction[] Get(int _nFuntionId, int _nRoleId)
        {
            SystemRoleActionPermission[] alist = SystemRoleActionPermission.GetRoleActions(_nRoleId, _nFuntionId);
            if (null == alist || alist.Length == 0)
                return null;
            List<string> ltActionId = new List<string>();
            foreach (SystemRoleActionPermission oPermission in alist)
                ltActionId.Add(oPermission.ActionId + "");
            string strFilter = string.Format("Id in ({0})", string.Join(",", ltActionId.ToArray()));
            return List(strFilter);
        }

        public static SystemAction[] GetFunctionAction(int _FunctionId)
        {
            if (_FunctionId <= 0)
                return null;
            SystemAction oGet = new SystemAction();
            SystemAction[] alist = (SystemAction[])HEntityCommon.HEntity(oGet).EntityList("FunctionId=" + _FunctionId);
            return alist;
        }

        public static SystemAction[] GetUserAction(int _nUserId, int _nFunctionId)
        {
            if (_nUserId <= 0 || _nFunctionId <= 0)
                return null;
            //1、获取用户继承自角色的Action
            SystemUserRole[] alUserRoles = SystemUserRole.GetUserRoles(_nUserId);
            if (null == alUserRoles || alUserRoles.Length <= 0)
                return null;
            List<SystemAction> ltAction = new List<SystemAction>();
            foreach (SystemUserRole oRole in alUserRoles)
            {
                SystemAction[] alist = Get(_nFunctionId, oRole.RoleId);
                if (null == alist || alist.Length == 0)
                    continue;
                foreach (SystemAction action in alist)
                {
                    if (ltAction.IndexOf(action) >= 0)
                        continue;
                    ltAction.Add(action);
                }
            }
            //2、获取用户自定义的Action
            SystemUserActionPermission[] alUserAction = SystemUserActionPermission.Get(_nUserId, _nFunctionId);
            if (null != alUserAction && alUserAction.Length > 0)
            {
                List<string> ltUserActionId = new List<string>();
                foreach (SystemUserActionPermission oPermission in alUserAction)
                    ltUserActionId.Add(oPermission.ActionId + "");
                string strFilter = string.Format("Id in ({0})", string.Join(",", ltUserActionId.ToArray()));
                SystemAction[] alUActions = List(strFilter);
                foreach (SystemAction action in alUActions)
                {
                    if (ltAction.Contains(action))
                        continue;
                    ltAction.Add(action);
                }
            }
            ltAction.Sort(new SystemAction());
            return ltAction.ToArray();
        }

        public static void Delete(int _nId)
        {
            SystemAction oDel = Get(_nId);
            if (null == oDel)
                return;
            //1、删除角色操作关系表
            SystemRoleActionPermission[] alRoleActions = SystemRoleActionPermission.GetByActionId(_nId);
            if (null != alRoleActions && alRoleActions.Length > 0)
            {
                foreach (SystemRoleActionPermission aPermission in alRoleActions)
                    SystemRoleActionPermission.Delete(aPermission.Id);
            }

            //2、删除用户操作关系表
            SystemUserActionPermission[] alUserActions = SystemUserActionPermission.GetByActionId(_nId);
            if (null != alUserActions && alUserActions.Length > 0)
            {
                foreach (SystemUserActionPermission aPermission in alUserActions)
                    SystemUserActionPermission.Delete(aPermission.Id);
            }
            
            //3、删除Action
            HEntityCommon.HEntity(oDel).EntityDelete();
        }

        public static SystemAction[] List()
        {
            SystemAction[] alist = (SystemAction[])HEntityCommon.HEntity(new SystemAction()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemAction[] List(string _strFilter)
        {
            SystemAction[] alist = (SystemAction[])HEntityCommon.HEntity(new SystemAction()).EntityList(_strFilter);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static SystemAction[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
        {
            SystemAction oList = new SystemAction();
            SystemAction[] alist = (SystemAction[])HEntityCommon.HEntity(oList).EntityList(__strFilter, "", __nPageIndex, __nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static int Save(SystemAction _saveObj)
        {
            if (null == _saveObj)
                return -1;
            return HEntityCommon.HEntity(_saveObj).EntitySave();
        }
    }
}
