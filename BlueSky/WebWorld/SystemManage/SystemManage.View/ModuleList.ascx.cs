using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;

using System.Web.UI.HtmlControls;
using WebBase.Utilities;
using BlueSky.EntityAccess;
using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class ModuleList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            _BindData();
        }

        private void _BindData()
        {
            SystemModule oModule = new SystemModule();
            PagerNavication.RecordsCount = EntityAccess<SystemModule>.Access.Count();
            SystemModule[] al = SystemModule.List("", "", PagerNavication.PageIndex, PagerNavication.PageSize);
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
                SystemModule oModule = (SystemModule)e.Item.DataItem;
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

        protected void btn_HiddenDelete_Click(object sender, EventArgs e)
        {
            string[] alSelectIds = (string[])PageUtil.PageSelectHiddenValue(this.Page, true);
            if (null == alSelectIds || alSelectIds.Length == 0)
                return;
            int nCount = 0;
            foreach (string id in alSelectIds)
            {
                int nId = TypeUtil.ParseInt(id, -1);
                if (nId <= 0)
                    continue;
                SystemModule.Delete(nId);
                nCount++;
            }
            _BindData();
            PageUtil.PageAlert(this.Page, string.Format("成功删除 {0} 个模块！", nCount));
        }
    }
}