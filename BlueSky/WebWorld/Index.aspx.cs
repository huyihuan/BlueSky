using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;

namespace WebWorld
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session过期后，进入登陆界面
            if (SystemUtil.GetCurrentUserId() <= 0)
            {
                Response.Redirect("Default.html", true);
                return;
            }
        }
    }
}
