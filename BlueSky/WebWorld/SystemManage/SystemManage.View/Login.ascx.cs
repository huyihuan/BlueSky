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
    public partial class Login : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strUserName = txt_UserName.Value.Trim();
            string strPassword = txt_Password.Value;
            string strVCode = txt_VCode.Value;
            if ("" == strUserName || "" == strPassword || "" == strVCode)
                return;
            if (!strVCode.ToLower().Equals(SystemUtil.VCodeGetCurrent().ToLower()))
            {
                PageUtil.PageAlert(this.Page, "验证码错误！");
                return;
            }

            if (!UserInformation.ExistUser(strUserName, strPassword))
            {
                PageUtil.PageAlert(this.Page, "用户名或密码错误！");
                return;
            }
            SaveLoginUserItem(strUserName);
            PageUtil.PageAppendScript(this.Page, "top.window.location.href = \"Index.aspx\";");
        }

        private void SaveLoginUserItem(string _strUserName)
        {
            UserInformation loginItem = UserInformation.Get(_strUserName);
            if (null == loginItem)
                return;
            SystemUtil.LoginUser(loginItem);

            //记录登陆日志
            SystemLog oLog = new SystemLog();
            oLog.UserId = loginItem.Id;
            oLog.AccessFunctionName = "系统登陆";
            oLog.AccessActionName = "系统登陆";
            oLog.AccessTime = DateTime.Now;
            oLog.AccessURL = this.Page.Request.Url.AbsoluteUri;
            oLog.Remark = "";
            SystemLog.Save(oLog);
        }
    }
}