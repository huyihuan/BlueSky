﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataBase;
using System.Web.UI.HtmlControls;
using WebSystemBase.SystemClass;
using WebSystemBase.UserControls;
using WebSystemBase.Utilities;

namespace WebWorld.SystemManage
{
    public partial class RoleList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            BindData();
        }

        protected void BindData()
        {
            SystemRole userObj = new SystemRole();
            PagerNavication.RecordsCount = DataBase.HEntityCommon.HEntity(userObj).EntityCount();
            SystemRole[] al = SystemRole.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
            rptItems.DataSource = al;
            rptItems.DataBind();
        }

        protected void PagerNavication_PagerIndexChanged(object sender, PagerIndexChagedEventArgs e)
        {
            BindData();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SystemRole RoleItem = (SystemRole)e.Item.DataItem;
                if (null == RoleItem)
                    return;
                PageUtil.PageFillView(e.Item, RoleItem);

                HtmlInputCheckBox cbSelect = e.Item.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null != cbSelect)
                    cbSelect.Value = RoleItem.Id + "";

                Label lbl = e.Item.FindControl("lbl_OrderId") as Label;
                lbl.Text = (PagerNavication.PageIndex - 1) * PagerNavication.PageSize + e.Item.ItemIndex + 1 + "";
            }
        }
    }
}