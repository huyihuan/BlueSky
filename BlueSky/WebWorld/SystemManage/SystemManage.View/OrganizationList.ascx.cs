using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.SystemClass;
using System.Web.UI.HtmlControls;
using WebSystemBase.Utilities;

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
                SystemOrganization oItem = (SystemOrganization)e.Item.DataItem;
                if (null == oItem)
                    return;
                PageUtil.PageFillView(e.Item, oItem);

                HtmlInputCheckBox cbSelect = e.Item.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null != cbSelect)
                    cbSelect.Value = oItem.Id + "";

                Label lbl = e.Item.FindControl("lbl_OrderId") as Label;
                lbl.Text = e.Item.ItemIndex + 1 + "";
            }
        }
    }
}