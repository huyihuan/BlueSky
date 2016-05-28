using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using WebWorld.Class;
using System.Collections;
using DataBase;

namespace WebWorld.FunctionControls.SystemManage
{
    public partial class FunctionsManage : System.Web.UI.UserControl
    {
        public string strTree = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            strTree = FunctionUtil.CreateFunctionTree();
        }
    }
}