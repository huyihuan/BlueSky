using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;
using System.Data;

namespace WebWorld.Modules.MyMusic
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
            using (BlueSkyEntities oEntity = new BlueSkyEntities())
            {
                foreach (int nId in alDeleteId)
                {
                    EntityKey oKey = new EntityKey("BlueSkyEntities.Music", "Id", nId);
                    oEntity.DeleteObject(oEntity.GetObjectByKey(oKey));
                }
                oEntity.SaveChanges();
            }
            PageUtil.PageAlert(this.Page, "删除成功！");
            PageUtil.PageAppendScript(this.Page, "top.refreshActiveWindow();top.windowFactory.closeTopFocusForm()");
        }
    }
}