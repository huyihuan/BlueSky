using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;
using WebSystemBase.SystemClass;
using System.Collections;

namespace WebWorld.SystemManage
{
    public partial class SetUserAction : System.Web.UI.UserControl
    {
        public int nFunctionId = -1, nUserId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nFunctionId = PageUtil.GetQueryInt(this.Request, "setfn", -1);
            nUserId = PageUtil.GetQueryInt(this.Request, "userid", -1);
            if (IsPostBack)
                return;
            _InitAction();
        }

        protected void _InitAction()
        {
            if (nFunctionId <= 0 || nUserId <= 0)
                return;
            SystemAction[] actionList = SystemAction.GetFunctionAction(nFunctionId);
            if (null == actionList || actionList.Length == 0)
                return;
            _InitExistPermission();
            cbl_Actions.Items.Clear();
            foreach (SystemAction oAction in actionList)
            {
                ListItem cb = new ListItem();
                cb.Text = "&nbsp;" + oAction.Name + "&nbsp;&nbsp;";
                cb.Value = oAction.Id + "";
                cb.Selected = htRoleAction.ContainsKey(oAction.Id) || htUserAction.ContainsKey(oAction.Id);
                cb.Enabled = !htRoleAction.ContainsKey(oAction.Id);
                if (htRoleAction.ContainsKey(oAction.Id))
                    cb.Attributes["title"] = "继承自角色，不能改变！";
                cbl_Actions.Items.Add(cb);
            }
        }

        public Hashtable htUserAction = new Hashtable();
        public Hashtable htRoleAction = new Hashtable();
        protected void _InitExistPermission()
        {
            if (nFunctionId <= 0 || nUserId <= 0)
                return;
            htUserAction.Clear();
            htRoleAction.Clear();
            SystemUserActionPermission[] aPermisionList = SystemUserActionPermission.Get(nUserId, nFunctionId);
            if (null != aPermisionList && aPermisionList.Length > 0)
            {
                foreach (SystemUserActionPermission oPermission in aPermisionList)
                    htUserAction[oPermission.ActionId] = oPermission;
            }

            SystemUserRole[] aRoleList = SystemUserRole.GetUserRoles(nUserId);
            if (null != aRoleList && aRoleList.Length > 0)
            {
                foreach (SystemUserRole uRole in aRoleList)
                {
                    SystemRoleActionPermission[] aRolePermissionList = SystemRoleActionPermission.GetRoleActions(uRole.Id, nFunctionId);
                    if (null == aRolePermissionList || aRolePermissionList.Length == 0)
                        continue;
                    foreach (SystemRoleActionPermission oPermission in aRolePermissionList)
                    {
                        if (htRoleAction.ContainsKey(oPermission.ActionId))
                            continue;
                        htRoleAction[oPermission.ActionId] = 1;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (nFunctionId <= 0 || nUserId <= 0)
                return;
            _InitExistPermission();
            foreach (ListItem cbSel in cbl_Actions.Items)
            {
                int nId = DataBase.Util.ParseInt(cbSel.Value, 0);
                if (!cbSel.Enabled || htRoleAction.ContainsKey(nId))
                    continue;
                if (!cbSel.Selected && htUserAction.ContainsKey(nId))
                {
                    SystemUserActionPermission.Delete(((SystemUserActionPermission)htUserAction[nId]).Id);
                }
                else if (cbSel.Selected && !htUserAction.ContainsKey(nId))
                {
                    SystemUserActionPermission oAdd = new SystemUserActionPermission();
                    oAdd.ActionId = nId;
                    oAdd.UserId = nUserId;
                    oAdd.FunctionId = nFunctionId;
                    SystemUserActionPermission.Save(oAdd);
                }
            }
            _InitAction();
            PageUtil.PageAlert(this.Page, "保存成功！");
        }
    }
}