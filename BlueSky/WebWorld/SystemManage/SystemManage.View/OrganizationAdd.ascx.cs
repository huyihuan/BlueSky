using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;
using WebBase.Utilities;
using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class OrganizationAdd : System.Web.UI.UserControl
    {
        public int nId = -1;
        public string strOrganizationTypeUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            strOrganizationTypeUrl = SystemUtil.ResovleActionFormURL("OrganizationTypeList");
            if (!IsPostBack)
            {
                btnRefresh_Click(null, null);
                _InitForm();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            SystemOrganizationType.BindList(sel_OrganizationTypeId, true, true, false, -1, true);
        }

        private void _InitForm() 
        {
            if (nId == -1)
                return;
            SystemOrganization oGet = SystemOrganization.Get(nId);
            if (null != oGet)
                PageUtil.PageFillEdit(this, oGet);
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strName = txt_Name.Value.Trim();
            if (nId <= 0 && SystemOrganization.Exist(strName))
            {
                PageUtil.PageAlert(this.Page, "该组织机构名称已存在！");
                return;
            }
            SystemOrganization addItem = SystemOrganization.Get(nId);
            if (null == addItem)
            {
                addItem = new SystemOrganization();
            }
            addItem.Name = strName;
            addItem.TypeId = TypeUtil.ParseInt(sel_OrganizationTypeId.SelectedValue, -1);
            addItem.Remark = txt_Remark.Text;
            if (SystemOrganization.Save(addItem) > 0)
            {
                PageUtil.PageAlert(this.Page, "保存成功！");
            }
        }
    }
}