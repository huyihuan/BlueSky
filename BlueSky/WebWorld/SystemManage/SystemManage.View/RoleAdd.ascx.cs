using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebBase.SystemClass;
using WebBase.Utilities;

namespace WebWorld.SystemManage
{
    public partial class RoleAdd : System.Web.UI.UserControl
    {
        public int nId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            if (!IsPostBack)
            {
                _InitForm();
            }
        }

        private void _InitForm()
        {
            if (nId == -1)
                return;
            SystemRole oGet = SystemRole.Get(nId);
            if (null != oGet)
                PageUtil.PageFillEdit(this, oGet);
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strRoleName = txt_Name.Value.Trim();
            string strRemark = txt_Description.Value.Trim();
            if (SystemRole.Exist(strRoleName))
            {
                if (nId <= 0)
                {
                    PageUtil.PageAlert(this.Page, string.Format("角色“{0}”已存在！", strRoleName));
                    return;
                }
                else
                {
                    SystemRole oExist = SystemRole.Get(nId);
                    if (strRoleName != oExist.Name)
                    {
                        PageUtil.PageAlert(this.Page, string.Format("角色“{0}”已存在！", strRoleName));
                        return;
                    }
                }
            }
            SystemRole addItem = SystemRole.Get(nId);
            if (null == addItem)
            {
                addItem = new SystemRole();
            }
            addItem.Name = strRoleName;
            addItem.Description = strRemark;
            int nNewId = SystemRole.Save(addItem);
            PageUtil.PageAlert(this.Page, nNewId > 0 ? "保存成功！" : "保存失败！");
            PageUtil.PageClosePopupWindow(this.Page, true);
        }
    }
}