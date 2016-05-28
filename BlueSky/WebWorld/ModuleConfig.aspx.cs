using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;

namespace WebWorld
{
    public partial class ModuleConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strFunctionPath = SystemUtil.ResovleControlPath("SystemManage", "ModuleUpdate");
            Control uc = LoadControl(strFunctionPath);
            ph.Controls.Add(uc);
        }
    }
}
