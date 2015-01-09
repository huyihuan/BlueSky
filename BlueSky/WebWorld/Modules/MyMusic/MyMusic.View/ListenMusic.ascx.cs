using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebWorld.Modules.MyMusic.Domain;
using WebWorld.Modules.MyMusic.Services;

namespace WebWorld.Modules.MyMusic.View
{
    public partial class ListenMusic : System.Web.UI.UserControl
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
                lit_embed.Text = string.Format("<embed src=\"{0}\" width=\"300\" height=\"300\" type=\"audio/mpeg\" loop=\"true\" autostart=\"true\" />", oMusic.MusicURL);
        }
    }
}