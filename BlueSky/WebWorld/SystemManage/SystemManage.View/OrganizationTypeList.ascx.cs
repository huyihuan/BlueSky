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

namespace WebWorld.SystemManage
{
    public partial class OrganizationTypeList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            _BindData();
        }

        private void _BindData()
        {
            PagerNavication.RecordsCount = EntityAccess<SystemOrganizationType>.Access.Count();
            SystemOrganizationType[] al = SystemOrganizationType.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
            rptItems.DataSource = al;
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
                SystemOrganizationType oEntity = (SystemOrganizationType)e.Item.DataItem;
                if (null == oEntity)
                    return;
                int nIndex = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1;
                PageUtil.PageFillListItem(e.Item, oEntity, nIndex, oEntity.Id.ToString());
            }
        }
    }
}