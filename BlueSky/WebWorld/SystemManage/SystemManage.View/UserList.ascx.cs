using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WebBase.SystemClass;
using WebBase.Utilities;
using BlueSky.EntityAccess;
using BlueSky.Extensions;

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
            string strFilter = "1=1";
            string strUserName = txt_UserName.Value.Trim();
            if (!strUserName.IsNullOrEmpty())
            {
                strFilter += string.Format(" and UserName like '%{0}%'", strFilter);
            }
            string strNickName = txt_NickName.Value.Trim();
            if (!strNickName.IsNullOrEmpty())
            {
                strFilter += string.Format(" and NickName like '%{0}%'", strNickName);
            }
            string strCardID = txt_CardID.Value.Trim();
            if (!strCardID.IsNullOrEmpty())
            {
                strFilter += string.Format(" and CardID like '%{0}%'", strCardID);
            }

            PagerNavication.RecordsCount = EntityAccess<UserInformation>.Access.Count(strFilter);
            UserInformation[] al = UserInformation.List(strFilter, "", PagerNavication.PageIndex, PagerNavication.PageSize);
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
                UserInformation oEntity = (UserInformation)e.Item.DataItem;
                if (null == oEntity)
                    return;
                int nIndex = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1;
                PageUtil.PageFillListItem(e.Item, oEntity, true, nIndex, true, oEntity.Id.ToString());
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            _BindData();
        }
    }
}