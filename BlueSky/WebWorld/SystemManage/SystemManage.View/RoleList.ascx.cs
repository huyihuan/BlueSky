using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using WebBase.SystemClass;
using WebBase.UserControls;
using WebBase.Utilities;
using BlueSky.EntityAccess;

namespace WebWorld.SystemManage
{
    public partial class RoleList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            BindData();
        }

        protected void BindData()
        {
            PagerNavication.RecordsCount = EntityAccess<SystemRole>.Access.Count();
            SystemRole[] al = SystemRole.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
            rptItems.DataSource = al;
            rptItems.DataBind();
        }

        protected void PagerNavication_PagerIndexChanged(object sender, PagerIndexChagedEventArgs e)
        {
            BindData();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SystemRole oEntity = (SystemRole)e.Item.DataItem;
                if (null == oEntity)
                    return;
                int nIndex = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1;
                PageUtil.PageFillListItem(e.Item, oEntity, nIndex, oEntity.Id.ToString());
            }
        }
    }
}