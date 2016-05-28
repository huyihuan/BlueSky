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
    public partial class SetUserActionFrame : System.Web.UI.UserControl
    {
        public string strTree = "";
        public string strActionUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            strActionUrl = SystemUtil.ResovleSingleFormUrl(this.Request, "SetUserAction", "userid=" + PageUtil.GetQueryInt(this.Request, "id", 0));
            if (!IsPostBack)
                strTree = SystemFunctionUtil.CreateFunctionTree();
        }
    }
}