using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;

using WebSystemBase.Utilities;

namespace WebWorld
{
    public partial class Menu : System.Web.UI.Page
    {
        public string strTree = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            string strFunctionPath = SystemUtil.ResovleControlPath("WebWorld.SystemManage", "FunctionsTree");
            Control cMenuTree = LoadControl(strFunctionPath);
            ph_MenuContainer.Controls.Add(cMenuTree);
            
        }
    }
}
