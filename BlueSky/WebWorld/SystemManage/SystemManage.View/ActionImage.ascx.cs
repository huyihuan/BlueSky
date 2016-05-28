using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using System.IO;
using System.Text;

namespace WebWorld.SystemManage
{
    public partial class ActionImage : System.Web.UI.UserControl
    {
        public string strActionImagePath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            strActionImagePath = SystemUtil.ActionImagePath;
            if (!IsPostBack)
            {
                _BindData();
            }
        }

        private void _BindData()
        {
            string strImagePath = Server.MapPath(SystemUtil.ActionImagePath);
            if (!Directory.Exists(strImagePath))
            {
                lbl_ImageList.Text = "图片加载失败！";
                return;
            }
            string[] astrImageNames = Directory.GetFiles(strImagePath);
            PagerNavication.RecordsCount = astrImageNames.Length;
            int nMinIndex = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize;
            int nMaxIndex = PagerNavication.PageIndex * PagerNavication.PageSize;
            if (null == astrImageNames || astrImageNames.Length == 0)
                return;
            StringBuilder sbImage = new StringBuilder();
            for (int i=nMinIndex;i<nMaxIndex;i++)
            {
                if (i >= PagerNavication.RecordsCount)
                    break;
                string strImageName = Path.GetFileName(astrImageNames[i]);
                sbImage.Append(string.Format("<div class='action-image-box'><a href='javascript:void(0);' onclick='selectedImage(\"{1}\");'><img src='{0}' align='absMiddle' /></a></div>", SystemUtil.ResovleActionImagePath(strImageName), strImageName));
            }
            lbl_ImageList.Text = sbImage.ToString();
        }

        protected void PagerNavication_PagerIndexChanged(object sender, WebBase.UserControls.PagerIndexChagedEventArgs e)
        {
            _BindData();
        }
    }
}