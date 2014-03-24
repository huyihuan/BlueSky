using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;
using System.Data.Objects;
using System.Data;

namespace WebWorld.Modules.MyMusic
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
            if (nId >= 0)
            {
                using (BlueSkyEntities oEntity = new BlueSkyEntities())
                {
                    EntityKey oKey = new EntityKey("BlueSkyEntities.Music", "Id", nId);
                    Music oMusic = oEntity.GetObjectByKey(oKey) as Music;
                    if (null != oMusic)
                    {
                        PageUtil.PageFillEdit(this, oMusic);
                    }
                }
            }
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            using (BlueSkyEntities oEntity = new BlueSkyEntities())
            {
                Music oMusic = new Music();
                if (nId > 0)
                {
                    EntityKey oKey = new EntityKey("BlueSkyEntities.Music", "Id", nId);
                    oMusic = oEntity.GetObjectByKey(oKey) as Music;
                }
                oMusic.MusicName = txt_MusicName.Value.Trim();
                oMusic.MusicURL = txt_MusicURL.Value.Trim();
                oMusic.MusicType = txt_MusicType.Value.Trim();

                if (nId <= 0)
                {
                    int nUserId = SystemUtil.GetCurrentUserId();
                    IQueryable<UserInformation> oUser = oEntity.UserInformation.Where(m => m.Id == nUserId);
                    if (null != oUser && oUser.Count() > 0)
                    {
                        oMusic.UserInformation = oUser.First();
                    }
                    oEntity.AddObject("Music", oMusic);
                }
                else
                {
                    oEntity.ApplyPropertyChanges("Music", oMusic);
                }
                oEntity.SaveChanges();
            }

            PageUtil.PageAlert(this.Page, "保存成功！");
            PageUtil.PageAppendScript(this.Page, "top.refreshActiveWindow();top.windowFactory.closeTopFocusForm()");
        }
    }
}