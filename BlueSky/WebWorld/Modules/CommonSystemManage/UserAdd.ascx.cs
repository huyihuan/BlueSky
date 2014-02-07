using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataBase;
using System.Collections;
using WebWorld.Modules.CommonSystemManage.Class;

namespace WebWorld.Modules.CommonSystemManage
{
    public partial class UserAdd : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            RoleItem.BindList(lb_RoleList, false, false, true);
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
            string strUserName = txt_UserName.Value.Trim();
            string strNickName = txt_NickName.Value.Trim();
            string strPassword = txt_Password.Value.Trim();
            string strPasswordSecond = txt_PasswordSecond.Value.Trim();
            if ("" == strUserName || "" == strNickName || "" == strPassword || "" == strPasswordSecond)
                return;
            if(UserItem.ExistUser(strUserName))
            {
                PageUtil.PageAlert(this.Page, "该用户名已存在！");
                return;
            }
            if (!strPassword.Equals(strPasswordSecond))
            {
                PageUtil.PageAlert(this.Page, "密码和确认密码不一致！");
                return;
            }
            if (lb_RoleSelect.Items.Count == 0)
            {
                PageUtil.PageAlert(this.Page, "请选择角色！");
                return;
            }
            UserItem addItem = new UserItem();
            addItem.UserName = strUserName;
            addItem.NickName = strNickName;
            addItem.Password = Util.MD5Encrypt(strPassword);
            addItem.Gender = rb_GenderMale.Checked ? "男" : "女";
            string strTel = txt_Tel.Value.Trim();
            if ("" != strTel)
                addItem.Tel = txt_Tel.Value.Trim();
            string strEmail = txt_Email.Value.Trim();
            if ("" != strEmail)
                addItem.Email = txt_Email.Value.Trim();
            int nNewId = UserItem.Save(addItem);
            if (nNewId <= 0)
                return;

            UserRoleItem[] alUserRoles = UserRoleItem.GetUserRoles(nNewId);
            Hashtable htExistRoles = new Hashtable();
            if (null != alUserRoles)
            {
                foreach (UserRoleItem item in alUserRoles)
                    htExistRoles[item.RoleItemId + ""] = 1;
            }
            foreach (ListItem item in lb_RoleSelect.Items)
            {
                if (htExistRoles.ContainsKey(item.Value))
                    continue;
                UserRoleItem addUserRole = new UserRoleItem();
                addUserRole.RoleItemId = int.Parse(item.Value);
                addUserRole.UserItemId = nNewId;
                UserRoleItem.Save(addUserRole);
            }
            PageUtil.PageAlert(this.Page, "保存成功！");
        }
    }
}