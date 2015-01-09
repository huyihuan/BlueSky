using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WebWorld.Modules.MyMusic.Domain;
using WebWorld.Modules.MyMusic.Services;
using WebBase.Utilities;

namespace WebWorld.Modules.MyMusic.View
{
    public partial class MyMusicManage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            _BindData();
        }

        private void _BindData()
        {
            string strFilter = txt_Filter.Value.Trim();
            if (!string.IsNullOrEmpty(strFilter))
                strFilter = string.Format("MusicName like '%{0}%' or MusicType like '%{0}%'", strFilter);
            Music[] aList = MusicServices.List(strFilter, "Id", PagerNavication.PageIndex, PagerNavication.PageSize);
            PagerNavication.RecordsCount = MusicServices.Count(strFilter);
            rptItems.DataSource = aList.ToArray();
            rptItems.DataBind();
        }

        protected void PagerNavication_PagerIndexChanged(object sender, WebBase.UserControls.PagerIndexChagedEventArgs e)
        {
            _BindData();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Music userItem = (Music)e.Item.DataItem;
                if (null == userItem)
                    return;
                PageUtil.PageFillView(e.Item, userItem);

                HtmlInputCheckBox cbSelect = e.Item.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null != cbSelect)
                    cbSelect.Value = userItem.Id + "";

                Label lbl = e.Item.FindControl("lbl_OrderId") as Label;
                lbl.Text = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1 + "";
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            _BindData();
        }
    }
}