using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;
using WebBase.Utilities;


namespace WebWorld.SystemManage
{
    public partial class SetRoleActionFrame : System.Web.UI.UserControl
    {
        public string strTree = "";
        public string strActionUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            strActionUrl = string.Format("{0}&roleid={1}", SystemUtil.ResovleSingleFormUrl(this.Request, "SetRoleAction"), PageUtil.GetQueryInt(this.Request, "id", 0));
            if (!IsPostBack)
                strTree = SystemFunctionUtil.CreateFunctionTree();
        }
    }
}