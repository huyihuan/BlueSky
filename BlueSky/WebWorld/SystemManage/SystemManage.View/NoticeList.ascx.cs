using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;
using System.Web.UI.HtmlControls;
using WebBase.Utilities;
using BlueSky.EntityAccess;
using WebBase.UserControls;

namespace WebWorld.SystemManage
{
    public partial class NoticeList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            _BindData();
        }

        private void _BindData()
        {
            PagerNavication.RecordsCount = EntityAccess<SystemNotice>.Access.Count();
            SystemNotice[] al = SystemNotice.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
            rptItems.DataSource = al;
            rptItems.DataBind();
        }

        protected void PagerNavication_PagerIndexChanged(object sender, PagerIndexChagedEventArgs e)
        {
            _BindData();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SystemNotice oEntity = (SystemNotice)e.Item.DataItem;
                if (null == oEntity)
                    return;
                int nIndex = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1;
                PageUtil.PageFillListItem(e.Item, oEntity, true, nIndex, true, oEntity.Id.ToString());
            }
        }
    }
}