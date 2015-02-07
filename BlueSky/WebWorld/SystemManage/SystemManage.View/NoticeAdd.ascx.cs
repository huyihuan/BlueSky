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
    public partial class NoticeAdd : System.Web.UI.UserControl
    {
        public int nId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            if (!IsPostBack)
            {
                _InitForm();
            }
        }

        private void _InitForm()
        {
            if (nId == -1)
                return;
            SystemNotice oGet = SystemNotice.Get(nId);
            if (null != oGet)
                PageUtil.PageFillEdit(this, oGet);
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strTitle = txt_Title.Value.Trim();
            string strContent = txt_Content.Value.Trim();
            SystemNotice addItem = SystemNotice.Get(nId);
            if (null == addItem)
            {
                addItem = new SystemNotice();
            }
            addItem.Title = strTitle;
            addItem.Content = strContent;
            DateTime dtBeginTime = new DateTime();
            DateTime.TryParse(txt_BeginTime.Value.Trim(), out dtBeginTime);
            addItem.BeginTime = dtBeginTime;
            DateTime.TryParse(txt_EndTime.Value.Trim(), out dtBeginTime);
            addItem.EndTime = dtBeginTime;

            addItem.RangeType = 0;
            addItem.TargetObject = 0;

            int nNewId = SystemNotice.Save(addItem);
            PageUtil.PageAlert(this.Page, nNewId > 0 ? "保存成功！" : "保存失败！");
            PageUtil.PageClosePopupWindow(this.Page, true);
        }
    }
}