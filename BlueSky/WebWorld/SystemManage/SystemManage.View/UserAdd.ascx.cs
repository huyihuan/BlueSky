using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using WebSystemBase.SystemClass;
using WebSystemBase.Utilities;

namespace WebWorld.SystemManage
{
    public partial class UserAdd : System.Web.UI.UserControl
    {
        public int nId = -1;
        public string strPermissionURL = "";
        public bool bIsPermission = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            bIsPermission = SystemUtil.IsFromPermission(this.Request);
            strPermissionURL = SystemUtil.ResovleActionFormURL(this.Request, "UserFunction", "id=" + PageUtil.GetQueryId(this.Request));
            if (IsPostBack)
                return;
            SystemRole.BindList(lb_RoleList, false, false, true);
            _InitForm();
        }

        private void _InitForm()
        {
            if (nId == -1)
                return;
            UserInformation oGet = UserInformation.Get(nId);
            if (null != oGet)
            {
                PageUtil.PageFillEdit(this, oGet);
                txt_UserName.Disabled = true;
                rb_GenderMale.Checked = oGet.Gender == 1;
                rb_GenderFemale.Checked = oGet.Gender == 2;
                SystemUserRole[] alUserRoles = SystemUserRole.GetUserRoles(oGet.Id);
                if (null != alUserRoles && alUserRoles.Length > 0)
                {
                    foreach (SystemUserRole item in alUserRoles)
                    {
                        ListItem li = new ListItem(item.PropertyRoleName, item.RoleId + "");
                        lb_RoleList.Items.Remove(li);
                        lb_RoleSelect.Items.Add(li);
                    }
                }
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
            string strUserName = txt_UserName.Value.Trim();
            string strNickName = txt_NickName.Value.Trim();
            if ("" == strUserName || "" == strNickName)
                return;
            if(nId <= 0 && UserInformation.ExistUser(strUserName))
            {
                PageUtil.PageAlert(this.Page, "该用户名已存在！");
                return;
            }
            if (lb_RoleSelect.Items.Count == 0)
            {
                PageUtil.PageAlert(this.Page, "请选择角色！");
                return;
            }
            UserInformation addItem = UserInformation.Get(nId);
            if (null == addItem)
            {
                addItem = new UserInformation();
                addItem.UserName = strUserName;
            }
            addItem.NickName = strNickName;
            //addItem.Password = Util.MD5Encrypt(UserInformation.CONST_strDefaultPassword);
            addItem.Gender = rb_GenderMale.Checked ? 1 : 2;
            addItem.QQ = txt_QQ.Value.Trim();
            addItem.Email = txt_Email.Value.Trim();
            int nNewId = UserInformation.Save(addItem);
            if (nNewId <= 0)
            {
                PageUtil.PageAlert(this.Page, "保存失败！");
                return;
            }

            SystemUserRole[] alUserRoles = SystemUserRole.GetUserRoles(nNewId);
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
                addUserRole.UserId = nNewId;
                SystemUserRole.Save(addUserRole);
            }
            PageUtil.PageAlert(this.Page, "保存成功！");
            PageUtil.PageClosePopupWindow(this.Page, true);
        }
    }
}