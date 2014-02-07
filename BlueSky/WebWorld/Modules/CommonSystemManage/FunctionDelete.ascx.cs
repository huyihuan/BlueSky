using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataBase;
using WebWorld.Modules.CommonSystemManage.Class;
namespace WebWorld.Modules.CommonSystemManage
{
    public partial class FunctionDelete : System.Web.UI.UserControl
    {
        int nDeleteId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nDeleteId = PageUtil.GetQueryInt(this.Request, "id", -1);
            if (IsPostBack)
                return;
            FunctionItem delObj = FunctionItem.Get(nDeleteId);
            if (null != delObj)
                lbl_DeleteMessage.Text = "确实要删除功能：" + delObj.Name + "？";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (nDeleteId <= 0)
                return;
            DataBase.HEntityCommon.HEntity(FunctionItem.Get(nDeleteId)).EntityDelete();
            PageUtil.PageAlert(this.Page, "删除成功！");
            PageUtil.PageAppendScript(this.Page, "top.windowFactory.closeTopFocusForm();");
        }
    }
}