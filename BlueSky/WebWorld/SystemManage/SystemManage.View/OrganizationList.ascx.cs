using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;
using System.Web.UI.HtmlControls;
using WebBase.Utilities;

namespace WebWorld.SystemManage
{
    public partial class OrganizationList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            _BindData();
        }

        private void _BindData()
        {
            SystemOrganization oModule = new SystemOrganization();
            SystemOrganization[] al = SystemOrganization.List();
            rptItems.DataSource = al;
            rptItems.DataBind();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SystemOrganization oEntity = (SystemOrganization)e.Item.DataItem;
                if (null == oEntity)
                    return;
                PageUtil.PageFillListItem(e.Item, oEntity, e.Item.ItemIndex + 1, oEntity.Id.ToString());
            }
        }
    }
}