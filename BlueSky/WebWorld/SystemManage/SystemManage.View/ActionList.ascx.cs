﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebBase.SystemClass;
using System.Web.UI.HtmlControls;
using BlueSky.EntityAccess;

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
            string strFilter = nFunctionId > 0 ? "FunctionId=" + nFunctionId : "";
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
                SystemAction oItem = (SystemAction)e.Item.DataItem;
                if (null == oItem)
                    return;
                PageUtil.PageFillView(e.Item, oItem);

                HtmlInputCheckBox cbSelect = e.Item.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null != cbSelect)
                    cbSelect.Value = oItem.Id + "";

                Label lbl = e.Item.FindControl("lbl_OrderId") as Label;
                lbl.Text = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1 + "";

                Label lblImage = e.Item.FindControl("lbl_IconName") as Label;
                lblImage.Text = string.Format("<img src='{0}' align='absMiddle' />", SystemUtil.ResovleActionImagePath(oItem.IconName));
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            _BindData();
        }
    }
}