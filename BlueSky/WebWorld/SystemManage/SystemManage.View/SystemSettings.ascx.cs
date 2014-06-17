using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueSky.DataAccess;

namespace WebWorld.SystemManage
{
    public partial class SystemSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGo_ServerClick(object sender, EventArgs e)
        {
            string strSql = txt_OperSql.Value.Trim();
            HDBOperation.QueryNonQuery(strSql);
            lit_Message.Text = "Success!";
        }

        protected void btn_Cancel_ServerClick(object sender, EventArgs e)
        {

        }
    }
}