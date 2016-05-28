using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DataBase;
using WebWorld.Modules.CommonSystemManage.Class;

namespace WebWorld.Modules.CommonSystemManage
{
    public partial class UserList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            BindData();
        }

        protected void BindData()
        {
            UserItem userObj = new UserItem();
            PagerNavication.RecordsCount = DataBase.HEntityCommon.HEntity(userObj).EntityCount();
            UserItem[] al = UserItem.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
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
                UserItem userItem = (UserItem)e.Item.DataItem;
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
    }
}