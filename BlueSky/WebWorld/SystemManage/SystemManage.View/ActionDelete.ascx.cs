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
    public partial class ActionDelete : System.Web.UI.UserControl
    {
        int[] alDeleteId = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            alDeleteId = PageUtil.GetQueryArrayIds(this.Request,-1);
            if (IsPostBack)
                return;
            if (null != alDeleteId && alDeleteId.Length >= 0)
            {
                lbl_DeleteMessage.Text = "确实要删除选中的 " + alDeleteId.Length + " 个操作？";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (null == alDeleteId && alDeleteId.Length <= 0)
                return;
            foreach(int nId in alDeleteId)
                SystemAction.Delete(nId);
            PageUtil.PageAlert(this.Page, "删除成功！");
            PageUtil.PageClosePopupWindow(this.Page, true);
        }
    }
}