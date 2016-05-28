using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;

using WebBase.SystemClass;
using System.Collections;

namespace WebWorld.SystemManage
{
    public partial class SetUserRole : System.Web.UI.UserControl
    {
        public int[] alIds;
        protected void Page_Load(object sender, EventArgs e)
        {
            alIds = PageUtil.GetQueryArrayIds(this.Request, -1);
            if (!IsPostBack)
            {
                lbl_Message.Text = string.Format("您将设置 <b>{0}</b> 个用户的角色：", alIds.Length);
                SystemRole.BindList(lb_RoleList, false, false, true);
            }
        }

        protected void btnSelectRole_Click(object sender, EventArgs e)
        {
            ListItem liSelect = lb_RoleList.SelectedItem;
            if (null == liSelect)
                return;
            lb_RoleSelect.SelectedIndex = -1;
            lb_RoleSelect.Items.Add(liSelect);
            lb_RoleList.Items.Remove(liSelect);
        }

        protected void btnRemoveRole_Click(object sender, EventArgs e)
        {
            ListItem liRemove = lb_RoleSelect.SelectedItem;
            if (null == liRemove)
                return;
            lb_RoleList.SelectedIndex = -1;
            lb_RoleList.Items.Add(liRemove);
            lb_RoleSelect.Items.Remove(liRemove);
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            if (lb_RoleSelect.Items.Count == 0)
            {
                PageUtil.PageAlert(this.Page, "请先选择角色后保存！");
                return;
            }
            if (alIds.Length == 0)
                return;
            foreach (int nId in alIds)
            {
                UserInformation addItem = UserInformation.Get(nId);
                if (null == addItem)
                    continue;
                SystemUserRole[] alUserRoles = SystemUserRole.GetUserRoles(nId);
                Hashtable htExistRoles = new Hashtable();
                if (null != alUserRoles)
                {
                    foreach (SystemUserRole item in alUserRoles)
                        htExistRoles[item.RoleId + ""] = item.Id;
                }
                foreach (string strKey in htExistRoles.Keys)
                {
                    if (null == lb_RoleSelect.Items.FindByValue(strKey))
                        SystemUserRole.Delete((int)htExistRoles[strKey]);
                }
                foreach (ListItem item in lb_RoleSelect.Items)
                {
                    if (htExistRoles.ContainsKey(item.Value))
                        continue;
                    SystemUserRole addUserRole = new SystemUserRole();
                    addUserRole.RoleId = int.Parse(item.Value);
                    addUserRole.UserId = nId;
                    SystemUserRole.Save(addUserRole);
                }
            }
            PageUtil.PageAlert(this.Page, "设置成功！");
        }

    }
}