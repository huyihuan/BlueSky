using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;

namespace WebWorld.SystemManage
{
    public partial class FunctionsTree : System.Web.UI.UserControl
    {
        public string strTree = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                strTree = SystemFunctionUtil.CreateFunctionTree();
        }
    }
}