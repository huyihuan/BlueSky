using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebWorld.SystemManage.SystemManage.View
{
    public partial class MFCOCXTest : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strContentPath = "~/Include/html/content.txt";
            string strPhysicalPath = Server.MapPath(strContentPath);
            string strContent = System.IO.File.ReadAllText(strPhysicalPath);
            //for (int i = 0; i < 1; i++)
            //{
            //    strContent += string.Format("<script type='text/javascript'>alert('{0}');</script>", i.ToString().PadRight(10, '0'));
            //}
            //strContent += "\r\n";
            //strContent += "\r\n";
            //strContent += "\r\n";
            //strContent += "\r\n";
            //this.Response.Write(strContent);
            //this.Response.Flush();
            //this.Response.ClearContent();
            //this.BussinessProcess();

        }

        public void BussinessProcess()
        {
            //System.Threading.Thread.Sleep(5 * 1000);
        }
    }
}