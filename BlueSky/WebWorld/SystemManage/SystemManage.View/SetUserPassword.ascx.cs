using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebBase.SystemClass;

using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class SetUserPassword : System.Web.UI.UserControl
    {
        public int[] alIds;
        protected void Page_Load(object sender, EventArgs e)
        {
            alIds = PageUtil.GetQueryArrayIds(this.Request, -1);
            if (!IsPostBack)
            {
                lbl_Message.Text = string.Format("您将设置 <b>{0}</b> 个用户的密码：", alIds.Length);
            }
        }
        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strPassword = txt_Password.Value.Trim();
            string strPasswordSecond = txt_PasswordSecond.Value.Trim();
            if ("" == strPassword || "" == strPasswordSecond)
                return;
            if (!strPassword.Equals(strPasswordSecond))
            {
                PageUtil.PageAlert(this.Page, "密码和确认密码不一致！");
                return;
            }
            string strMD5Password = CryptUtil.MD5Encrypt(strPassword);
            foreach (int nId in alIds)
            {
                UserInformation addItem = UserInformation.Get(nId);
                if (null == addItem)
                    continue;
                addItem.Password = strMD5Password;
                UserInformation.Save(addItem);
            }
            PageUtil.PageAlert(this.Page, "设置成功！");
        }

    }
}