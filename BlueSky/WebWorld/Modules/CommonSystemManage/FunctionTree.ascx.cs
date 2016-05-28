using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebWorld.Modules.CommonSystemManage.Class;

namespace WebWorld.Modules.CommonSystemManage
{
    public partial class FunctionTree : System.Web.UI.UserControl
    {
        public string strTree = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                strTree = FunctionUtil.CreateFunctionTree();
        }
    }
}