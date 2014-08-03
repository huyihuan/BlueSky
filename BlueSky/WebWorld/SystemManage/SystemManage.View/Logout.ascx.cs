using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebBase.SystemClass;

namespace WebWorld.SystemManage
{
    public partial class Logout : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //记录退出日志
            SystemLog oLog = new SystemLog();
            oLog.UserId = SystemUtil.GetCurrentUserId();
            oLog.AccessFunctionName = "退出系统";
            oLog.AccessActionName = "退出";
            oLog.AccessTime = DateTime.Now;
            oLog.AccessURL = this.Page.Request.Url.AbsoluteUri;
            oLog.Remark = "用户安全退出系统";
            SystemLog.Save(oLog);

            SystemUtil.LogoutUser();
            PageUtil.PageRefreshLayout(this.Page);
        }
    }
}