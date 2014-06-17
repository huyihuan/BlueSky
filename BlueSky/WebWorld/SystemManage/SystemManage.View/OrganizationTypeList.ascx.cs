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
            SystemOrganizationType oModule = new SystemOrganizationType();
            PagerNavication.RecordsCount = HEntityCommon.HEntity(oModule).EntityCount();
            SystemOrganizationType[] al = SystemOrganizationType.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
            rptItems.DataSource = al;
            rptItems.DataBind();
        }

        protected void PagerNavication_PagerIndexChanged(object sender, WebSystemBase.UserControls.PagerIndexChagedEventArgs e)
        {
            _BindData();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SystemOrganizationType oModule = (SystemOrganizationType)e.Item.DataItem;
                if (null == oModule)
                    return;
                PageUtil.PageFillView(e.Item, oModule);

                HtmlInputCheckBox cbSelect = e.Item.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null != cbSelect)
                    cbSelect.Value = oModule.Id + "";

                Label lbl = e.Item.FindControl("lbl_OrderId") as Label;
                lbl.Text = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1 + "";
            }
        }
    }
}