using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;
using WebBase.Utilities;
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
            PagerNavication.RecordsCount = EntityAccess<SystemLog>.Access.Count();
            SystemLog[] al = SystemLog.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
            rptItems.DataSource = al;
            rptItems.DataBind();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SystemLog oEntity = (SystemLog)e.Item.DataItem;
                if (null == oEntity)
                    return;
                int nIndex = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1;
                PageUtil.PageFillListItem(e.Item, oEntity, true, nIndex, true, oEntity.Id.ToString());

                Literal litURL = e.Item.FindControl("lit_AccessURL") as Literal;
                if (null != litURL)
                    litURL.Text = StringUtil.FixLegth(oEntity.AccessURL, 40);
            }
        }
        protected void PagerNavication_PagerIndexChanged(object sender, WebBase.UserControls.PagerIndexChagedEventArgs e)
        {
            _BindData();
        }

    }
}