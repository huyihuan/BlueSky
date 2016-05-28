using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebWorld.Class;
using DataBase;

namespace WebWorld.FunctionControls.SystemManage
{
    public partial class RoleAdd : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strRoleName = txt_RoleName.Value.Trim();
            string strRemark = txt_Remark.Value.Trim();
            RoleItem existObj = new RoleItem();
            existObj.RoleName = strRoleName;
            int nExist = DataBase.HEntityCommon.HEntity(existObj).EntityCount();
            if (nExist > 1)
            {
                PageUtil.PageAlert(this.Page, string.Format("RoleItem(“{0}”) exist mutil records！", strRoleName));
                return;
            }
            if (nExist == 1)
            {
                PageUtil.PageAlert(this.Page, string.Format("角色“{0}”已存在！", strRoleName));
                return;
            }
            RoleItem addItem = new RoleItem();
            addItem.RoleName = strRoleName;
            addItem.Remark = strRemark;
            DataBase.HEntityCommon.HEntity(addItem).EntitySave();
            PageUtil.PageAlert(this.Page, "保存成功！");
        }
    }
}