using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using System.IO;
using WebWorld.Modules.MyMusic.Services;
using WebWorld.Modules.MyMusic.Domain;

namespace WebWorld.Modules.MyMusic.View
{
    public partial class MusicUpload : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strValue = file_MusicFullName.Value;
            if (string.IsNullOrEmpty(strValue))
                return;
            int nUserId = SystemUtil.GetCurrentUserId();
            string strVirtualPath = MusicServices.GetUserMusicPath(nUserId) + "\\" + strValue;
            string strServerFullName = Server.MapPath(strVirtualPath);
            file_MusicFullName.PostedFile.SaveAs(strServerFullName);
            Music oMusic = new Music();
            oMusic.MusicName = Path.GetFileNameWithoutExtension(strValue);
            oMusic.MusicType = Path.GetExtension(strValue).ToLower().Substring(1);
            oMusic.MusicURL = strVirtualPath;
            oMusic.UserId = nUserId;
            MusicServices.Save(oMusic);
            PageUtil.PageAlert(this.Page, "上传完成!");
        }
    }
}