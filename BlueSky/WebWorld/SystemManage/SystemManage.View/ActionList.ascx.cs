using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebBase.SystemClass;
using System.Web.UI.HtmlControls;
using BlueSky.EntityAccess;
using BlueSky.Extensions;

namespace WebWorld.SystemManage
{
    public partial class ActionList : System.Web.UI.UserControl
    {
        public int nFunctionId = -1;
        public string strFunctionSelectUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            nFunctionId = PageUtil.GetQueryInt(this.Request, "setfn", -1);
            strFunctionSelectUrl = SystemUtil.ResovleSingleFormUrl(this.Request, "FunctionSelect");
            if (IsPostBack)
                return;
            _BindData();
            PageUtil.PageSetExtraParameters(this.Page, "setfn=" + nFunctionId);
        }

        private void _BindData()
        {
            string strFilter = "1=1" + (nFunctionId > 0 ? " and FunctionId=" + nFunctionId : "");
            string strName = txt_Name.Value.Trim();
            string strKey = txt_Key.Value.Trim();
            if (!strName.IsNullOrEmpty())
                strFilter += string.Format(" and Name like '%{0}%'", strName);
            if (!strKey.IsNullOrEmpty())
                strFilter += string.Format(" and [Key] like '%{0}%'", strKey);
            PagerNavication.RecordsCount = EntityAccess<SystemAction>.Access.Count(strFilter);
            SystemAction[] al = SystemAction.List(strFilter, "", PagerNavication.PageIndex, PagerNavication.PageSize);
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
                SystemAction oEntity = (SystemAction)e.Item.DataItem;
                if (null == oEntity)
                    return;
                int nIndex = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1;
                PageUtil.PageFillListItem(e.Item, oEntity, nIndex, oEntity.Id.ToString());

                Literal lblImage = e.Item.FindControl("lit_IconName") as Literal;
                lblImage.Text = string.Format("<img src='{0}' align='absMiddle' />", SystemUtil.ResovleActionImagePath(oEntity.IconName));
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            _BindData();
        }
    }
}