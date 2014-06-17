using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;
using System.Data;
using WebWorld.Modules.MyMusic.Domain;
using WebWorld.Modules.MyMusic.Services;

namespace WebWorld.Modules.MyMusic.View
{
    public partial class MusicAdd : System.Web.UI.UserControl
    {
        int nId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryId(this.Request, -1);
            if (IsPostBack)
                return;
            _InitForm();
        }

        private void _InitForm()
        {
            Music oMusic = MusicServices.Get(nId);
            if (null != oMusic)
                PageUtil.PageFillEdit(this, oMusic);
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            Music oMusic = MusicServices.Get(nId);
            if (null == oMusic)
            {
                oMusic = new Music();
                oMusic.UserId = SystemUtil.GetCurrentUserId();
            }
            PageUtil.PageFillEntity(this, oMusic);
            MusicServices.Save(oMusic);
            PageUtil.PageAlert(this.Page, "保存成功！");
            PageUtil.PageClosePopupWindow(this.Page, true);
        }
    }
}