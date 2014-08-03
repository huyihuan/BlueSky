using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebBase.SystemClass;

namespace WebWorld
{
    public partial class Index : System.Web.UI.Page
    {
        public string strUserInfomationURL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int nUserId = SystemUtil.GetCurrentUserId();
                UserInformation oUser = UserInformation.Get(nUserId);
                if (nUserId <= 0 || null == oUser)
                {
                    Response.Redirect("Default.html", true);
                    return;
                }
                lt_CurrentUserName.Text = string.Format("{0}[{1}]", oUser.UserName, oUser.NickName);
                strUserInfomationURL = SystemUtil.ResovleSingleFormUrl("SystemManage", "UserAdd", "&id=" + nUserId);

                //记录用户的用户ViewState
                ViewState["SessionKey_UserId"] = nUserId;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            int nUserId = null != ViewState["SessionKey_UserId"] ? (int)ViewState["SessionKey_UserId"] : -1;
            UserInformation oUser = UserInformation.Get(nUserId);
            if (null != oUser)
            {
                SystemUtil.LoginUser(oUser);
            }
            else
            {
                Response.Redirect("Default.html", true);
                return;
            }
        }
    }
}
