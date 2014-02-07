using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataBase;
using WebSystemBase.SystemClass;
using WebSystemBase.Utilities;

namespace WebWorld.SystemManage
{
    public partial class FunctionDelete : System.Web.UI.UserControl
    {
        int nDeleteId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nDeleteId = PageUtil.GetQueryInt(this.Request, "id", -1);
            if (IsPostBack)
                return;
            SystemFunction oFunction = SystemFunction.Get(nDeleteId);
            if (null != oFunction)
            {
                if(oFunction.ParentId == -1)
                {
                    lbl_DeleteMessage.Text = "模块不能删除，请在“系统模块管理”中删除！";
                    btnDelete.Visible = false;
                }
                else
                    lbl_DeleteMessage.Text = "确实要删除功能：" + oFunction.Name + "？";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (nDeleteId <= 0)
                return;
            SystemFunction.Delete(nDeleteId);
            PageUtil.PageAlert(this.Page, "删除成功！");
            PageUtil.PageAppendScript(this.Page, "top.windowFactory.closeTopFocusForm();");
        }
    }
}