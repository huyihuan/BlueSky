using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebWorld
{
    public partial class DesktopMain : System.Web.UI.Page
    {
        public string strDesktopFunctions = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            __InitDesktop();
        }

        private void __InitDesktop()
        {
            FunctionItem[] funcRoots = FunctionItem.GetFunctions(-1, false);
            int nNum = funcRoots.Length;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < nNum; i++)
            {
                sb.Append(string.Format("<div class='desktop-function' id='desktopFunction{0}'", i));
                string strClickEvent = string.Format("showWindow('Window.aspx?value={0}','{1}',400,300);", funcRoots[i].Value, funcRoots[i].Name);
                sb.Append(string.Format(" onclick=\"{0}\"><a class='function-bg'>{1}</a></div>", strClickEvent, funcRoots[i].Name));
            }
            strDesktopFunctions = sb.ToString();
        }
    }
}
