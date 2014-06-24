using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;
using WebSystemBase.SystemClass;

namespace WebWorld
{
    public partial class Index : System.Web.UI.Page
    {
        public string strUserInfomationURL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session过期后，进入登陆界面
            int nUserId = SystemUtil.GetCurrentUserId();
            if (SystemUtil.GetCurrentUserId() <= 0)
            {
                Response.Redirect("Default.html", true);
                return;
            }
            UserInformation oUser = UserInformation.Get(nUserId);
            lt_CurrentUserName.Text = string.Format("{0}[{1}]", oUser.UserName, oUser.NickName);
            strUserInfomationURL = SystemUtil.ResovleSingleFormUrl("SystemManage", "UserAdd", "&id=" + nUserId);
        }
    }
}
