using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WebSystemBase.SystemClass;
using WebSystemBase.Utilities;

namespace WebWorld.SystemManage
{
    public partial class UserList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            _BindData();
        }

        private void _BindData()
        {
            string strFilter =  txt_Filter.Value.Trim();
            UserInformation userObj = new UserInformation();
            PagerNavication.RecordsCount = DataBase.HEntityCommon.HEntity(userObj).EntityCount();
            if (!string.IsNullOrEmpty(strFilter))
                strFilter = string.Format("UserName like '%{0}%' or NickName like '%{0}%' or Email like '%{0}%' or QQ like '%{0}%' or CardID like '%{0}%'", strFilter);
            UserInformation[] al = UserInformation.List(strFilter, "", PagerNavication.PageIndex, PagerNavication.PageSize);
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
                UserInformation userItem = (UserInformation)e.Item.DataItem;
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

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            _BindData();
        }
    }
}