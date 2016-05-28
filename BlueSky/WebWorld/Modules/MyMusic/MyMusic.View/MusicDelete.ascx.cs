using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using System.Data;
using WebWorld.Modules.MyMusic.Services;

namespace WebWorld.Modules.MyMusic.View
{
    public partial class MusicDelete : System.Web.UI.UserControl
    {
        int[] alDeleteId = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            alDeleteId = PageUtil.GetQueryArrayIds(this.Request, -1);
            if (IsPostBack)
                return;
            if (null != alDeleteId && alDeleteId.Length >= 0)
            {
                lbl_DeleteMessage.Text = "确实要删除选中的 " + alDeleteId.Length + " 个音乐吗？";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (null == alDeleteId && alDeleteId.Length <= 0)
                return;
            foreach (int nId in alDeleteId)
            {
                MusicServices.Delete(nId);
            }
            PageUtil.PageAlert(this.Page, "删除成功！");
            PageUtil.PageClosePopupWindow(this.Page, true);
        }
    }
}