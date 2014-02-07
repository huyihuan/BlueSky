using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using WebSystemBase.Utilities;

namespace WebWorld.FunctionControls
{
    public partial class CodeTest : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = new DateTime(long.Parse(txt_Input.Value.Trim()));
                txt_Output.Value = dt.ToLongDateString();
            }
            catch
            {
                //DataBase.PageUtil.PageAlert(this.Page, "操作失败!");
                PageUtil.PageAppendScript(this.Page, "top.windowFactory.topAlert(\"操作失败！\");");
            }
        }

        protected void btnTicks_Click(object sender, EventArgs e)
        {
            System.Web.UI.Page page = this.Page;
            try
            {
                DateTime dt = DateTime.Parse(txt_Input.Value.Trim());
                txt_Output.Value = dt.Ticks + "";
            }
            catch
            {
                PageUtil.PageAlert(this.Page, "操作失败!");
            }
        }

        protected void btnMD5_Click(object sender, EventArgs e)
        {
            try
            {
                txt_Output.Value = DataBase.Util.MD5Encrypt(txt_Input.Value.Trim());
            }
            catch
            {
                PageUtil.PageAlert(this.Page, "操作失败!");
            }
        }
    }
}