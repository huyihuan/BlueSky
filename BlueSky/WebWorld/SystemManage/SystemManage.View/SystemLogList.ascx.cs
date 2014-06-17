using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.SystemClass;
using WebSystemBase.Utilities;
using System.Web.UI.HtmlControls;

using BlueSky.EntityAccess;
using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class SystemLogList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            _BindData();
        }

        private void _BindData()
        {
            SystemLog oModule = new SystemLog();
            PagerNavication.RecordsCount = HEntityCommon.HEntity(oModule).EntityCount("");
            SystemLog[] al = SystemLog.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
            rptItems.DataSource = al;
            rptItems.DataBind();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SystemLog oItem = (SystemLog)e.Item.DataItem;
                if (null == oItem)
                    return;
                PageUtil.PageFillView(e.Item, oItem);

                HtmlInputCheckBox cbSelect = e.Item.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null != cbSelect)
                    cbSelect.Value = oItem.Id + "";

                Literal litURL = e.Item.FindControl("lit_AccessURL") as Literal;
                if (null != litURL)
                    litURL.Text = StringUtil.FixLegth(oItem.AccessURL, 40);

                Literal litOrderId = e.Item.FindControl("lit_OrderId") as Literal;
                if(null != litOrderId)
                    litOrderId.Text = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1 + "";
            }
        }
        protected void PagerNavication_PagerIndexChanged(object sender, WebSystemBase.UserControls.PagerIndexChagedEventArgs e)
        {
            _BindData();
        }

    }
}