using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;
using System.IO;
using WebWorld.Modules.MyMusic.Services;

namespace WebWorld.Modules.MyMusic
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
            string strServerFullName =Server.MapPath(MusicServices.GetUserMusicPath(nUserId)) + "\\" + file_MusicFullName.Value;
            file_MusicFullName.PostedFile.SaveAs(strServerFullName);
            Music oMusic = new Music();
            oMusic.MusicName = Path.GetFileNameWithoutExtension(strValue);
            oMusic.MusicType = Path.GetExtension(strValue).ToLower().Substring(1);
            oMusic.MusicURL = strServerFullName;
            oMusic.UserInformation = UserInformationServices.GetObjectByKey(nUserId);
            MusicServices.Save(oMusic);
            PageUtil.PageAlert(this.Page, "上传完成!");
        }
    }
}