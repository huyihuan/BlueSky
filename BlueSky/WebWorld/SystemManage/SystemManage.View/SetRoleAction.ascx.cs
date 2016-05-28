using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebBase.SystemClass;
using System.Collections;
using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class SetRoleAction : System.Web.UI.UserControl
    {
        public int nFunctionId = -1, nRoleId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nFunctionId = PageUtil.GetQueryInt(this.Request, "setfn", -1);
            nRoleId = PageUtil.GetQueryInt(this.Request, "roleid", -1);
            if (IsPostBack)
                return;
            _InitAction();
        }

        protected void _InitAction()
        {
            if (nFunctionId <= 0 || nRoleId <= 0)
                return;
            SystemAction[] actionList = SystemAction.GetFunctionAction(nFunctionId);
            if (null == actionList || actionList.Length == 0)
            {
                btnSave.Disabled = true;
                return;
            }
            _InitExistPermission();
            cbl_Actions.Items.Clear();
            foreach (SystemAction oAction in actionList)
            {
                ListItem cb = new ListItem();
                cb.Text = "&nbsp;" + oAction.Name + "&nbsp;&nbsp;";
                cb.Value = oAction.Id + "";
                cb.Selected = htAction.ContainsKey(oAction.Id);
                cbl_Actions.Items.Add(cb);
            }
            
        }

        public Hashtable htAction = new Hashtable();
        protected void _InitExistPermission()
        {
            if (nFunctionId <= 0 || nRoleId <= 0)
                return;
            htAction.Clear();
            SystemRoleActionPermission[] aPermisionList = SystemRoleActionPermission.GetRoleActions(nRoleId, nFunctionId);
            if (null != aPermisionList && aPermisionList.Length > 0)
            {
                foreach (SystemRoleActionPermission oPermission in aPermisionList)
                    htAction[oPermission.ActionId] = oPermission;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (nFunctionId <= 0 || nRoleId <= 0)
                return;
            _InitExistPermission();
            foreach (ListItem cbSel in cbl_Actions.Items)
            {
                int nId = TypeUtil.ParseInt(cbSel.Value, 0);
                if (!cbSel.Selected && htAction.ContainsKey(nId))
                {
                    SystemRoleActionPermission.Delete(((SystemRoleActionPermission)htAction[nId]).Id);
                }
                else if (cbSel.Selected && !htAction.ContainsKey(nId))
                {
                    SystemRoleActionPermission oAdd = new SystemRoleActionPermission();
                    oAdd.ActionId = nId;
                    oAdd.RoleId = nRoleId;
                    oAdd.FunctionId = nFunctionId;
                    SystemRoleActionPermission.Save(oAdd);
                }
            }
            _InitAction();
            PageUtil.PageAlert(this.Page, "保存成功！");
        }
    }
}