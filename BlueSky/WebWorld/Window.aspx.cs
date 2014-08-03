using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using WebBase.SystemClass;
using WebBase.Utilities;

namespace WebWorld
{
    public partial class Window : System.Web.UI.Page
    {
        //public int nFunctionId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session过期后，进入登陆界面
            int nCurrrentUserId = SystemUtil.GetCurrentUserId();
            if (nCurrrentUserId <= 0)
            {
                Control loginControl = TemplateControl.LoadControl(SystemUtil.ResovleControlPath("SystemManage", "Login"));
                ph.Controls.Add(loginControl);
                return;
            }

            int nFunctionId = PageUtil.GetQueryInt(this.Request, "fn", -1);
            int nModuleId = PageUtil.GetQueryInt(this.Request, "m", -1);
            if (nFunctionId <= 0 && nModuleId <= 0)
                return;
            SystemFunction oFunction = new SystemFunction();
            if (nFunctionId >= 1)
            {
                oFunction = SystemFunction.Get(nFunctionId);
                if (null == oFunction)
                    return;
                nModuleId = oFunction.ModuleId;
            }
            SystemModule oModule = SystemModule.Get(nModuleId);
            if (null == oModule)
                return;
            string strSingleForm = PageUtil.GetQueryString(this.Request, "fm");
            string strActionKey = PageUtil.GetQueryString(this.Request, "akey");
            string strControlName = strSingleForm;
            SystemAction[] alActions = null;
            if (string.IsNullOrEmpty(strControlName))
            {
                alActions = SystemAction.GetUserAction(nCurrrentUserId, nFunctionId);
                if (null == alActions)
                    return;
                foreach (SystemAction action in alActions)
                {
                    if (!string.IsNullOrEmpty(strActionKey) && action.Key.Equals(strActionKey))
                    {
                        strControlName = action.ControlName;
                        break;
                    }
                    else if (string.IsNullOrEmpty(strActionKey) && action.IsDefault == 1)
                    {
                        strControlName = action.ControlName;
                        strActionKey = action.Key;
                        break;
                    }
                }

                if (!IsPostBack)
                {
                    //记录用户操作日志
                    SystemLog oLog = new SystemLog();
                    oLog.UserId = nCurrrentUserId;
                    oLog.AccessFunctionName = oFunction.Name;
                    oLog.AccessActionName = SystemAction.Get(strActionKey).Name;
                    oLog.AccessTime = DateTime.Now;
                    oLog.AccessURL = this.Page.Request.Url.AbsoluteUri;
                    oLog.Remark = string.Format("[ControlName：{0}][IP:{1}]", strControlName, this.Request.ServerVariables["REMOTE_ADDR"]);
                    SystemLog.Save(oLog);

                }
            }

            string strOtherUrl = "";
            if (!string.IsNullOrEmpty(strOtherUrl) && strOtherUrl.Length > 4 && strOtherUrl.Substring(0, 4) == "http")
            {
                Response.Write(string.Format("<iframe frameborder=\"0\" src=\"{0}\" scrolling=\"auto\" height=\"100%\" width=\"100%\"></iframe>", strOtherUrl));
                return;
            }

            string strControlPath = string.IsNullOrEmpty(strOtherUrl) ? SystemUtil.ResovleControlPath(oModule.Controller, strControlName) : strOtherUrl;
            Control loadControl = null;
            try
            {
                loadControl = LoadControl(strControlPath);
                ph.Controls.Add(loadControl);
            }
            catch(Exception ex)
            {
                Label lbHint = new Label();
                lbHint.Text = string.Format("<div style='width:100%;height:100%;text-align:center;color:#ff0000;'>控件加载失败!<br /><br />原因：{0}</div>", ex.Message);
                ph.Controls.Add(lbHint);
                return;
            }

            Control toolBar = loadControl.FindControl("toolBar");
            if (null != toolBar)
            {
                (toolBar as HtmlControl).Attributes["class"] = "toolBar";
                foreach (SystemAction action in alActions)
                {
                    HyperLink toolAction = new HyperLink();
                    toolAction.ID = "Action_" + action.Key;
                    toolAction.CssClass = action.Key == strActionKey ? "action-active" : "action-normal";
                    toolAction.ToolTip = action.Tip;
                    toolAction.Attributes["onclick"] = action.Key == strActionKey ? "void(0);" : string.Format("formUtil.actionDone({0});", action.ToActionJson());
                    toolAction.NavigateUrl = "javascript:void(0);";

                    HtmlGenericControl spanText = new HtmlGenericControl("span");
                    spanText.InnerText = action.Name;

                    HtmlImage toolIcon = new HtmlImage(); //图标必须是16像素宽高
                    toolIcon.Width = 16;
                    toolIcon.Height = 16;
                    toolIcon.Align = "absMiddle";
                    toolIcon.Attributes["hspace"] = "2";
                    toolIcon.Src = SystemUtil.ResovleActionImagePath(action.IconName);

                    toolAction.Controls.Add(toolIcon);
                    toolAction.Controls.Add(spanText);
                    toolBar.Controls.Add(toolAction);
                }
            }

            //页面刷新后清空选择的复选框
            hiddenSelectedValue.Value = "";

        }
    }
}
