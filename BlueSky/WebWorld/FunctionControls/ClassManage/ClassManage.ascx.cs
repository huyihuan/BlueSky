using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebWorld.Class;
using DataBase;

namespace WebWorld.FunctionControls.ClassManage
{
    public partial class ClassManage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            BindData();
        }

        protected void BindData()
        {
            ClassItem cObj = new ClassItem();
            PagerNavication.RecordsCount = DataBase.HEntityCommon.HEntity(cObj).EntityCount();
            ClassItem[] al = ClassItem.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
            rptItems.DataSource = al;
            rptItems.DataBind();
        }

        protected void PagerNavication_PagerIndexChanged(object sender, DataBase.UserControls.PagerIndexChagedEventArgs e)
        {
            BindData();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ClassItem oItem = (ClassItem)e.Item.DataItem;
                if (null == oItem)
                    return;
                PageUtil.PageFillView(e.Item, oItem);

                Label lbl = e.Item.FindControl("lbl_OrderId") as Label;
                lbl.Text = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1 + "";
            }
        }
    }
}