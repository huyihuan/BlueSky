using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebBase.SystemClass;

using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class OrganizationTypeAdd : System.Web.UI.UserControl
    {
        public int nId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            if (!IsPostBack)
            {
                ListItem[] alItems = new ListItem[1] { new ListItem("根类型", "-1") };
                SystemOrganizationType.BindList(sel_OrganizationTypeId, false, true, false, -1, true, alItems);
                _InitForm();
            }
        }

        private void _InitForm()
        {
            if (nId == -1)
                return;
            SystemOrganizationType oGet = SystemOrganizationType.Get(nId);
            if (null != oGet)
            {
                PageUtil.PageFillEdit(this, oGet);
                PageUtil.SetSelectValue(sel_OrganizationTypeId, oGet.ParentId + "");
            }
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strName = txt_Name.Value.Trim();
            if (nId <= 0 && SystemOrganizationType.Exist(strName))
            {
                PageUtil.PageAlert(this.Page, "该类型已存在！");
                return;
            }
            SystemOrganizationType addItem = SystemOrganizationType.Get(nId);
            if (null == addItem)
            {
                addItem = new SystemOrganizationType();
            }
            addItem.Name = strName;
            addItem.ParentId = TypeUtil.ParseInt(sel_OrganizationTypeId.SelectedValue, -1);
            addItem.Remark = txt_Remark.Text;
            int nAddId = SystemOrganizationType.Save(addItem);
            PageUtil.PageAlert(this.Page, nAddId > 0 ? "保存成功！" : "保存失败！");
        }
    }
}