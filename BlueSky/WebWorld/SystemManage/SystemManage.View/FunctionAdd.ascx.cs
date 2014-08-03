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
    public partial class FunctionAdd : System.Web.UI.UserControl
    {
        int nId = -1, nParentId = -1;
        public string strImageFormUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            nParentId = PageUtil.GetQueryInt(this.Request, "parentid", -1);
            strImageFormUrl = SystemUtil.ResovleSingleFormUrl(this.Request, "ActionImage");
            if (!IsPostBack)
                InitForm();
        }

        protected void InitForm()
        {
            if (nParentId <= 0)
            {
                PageUtil.PageAlert(this.Page, "请选择父级功能后新增功能！");
                PageUtil.PageAppendScript(this.Page, "top.windowFactory.closeTopFocusForm();");
                return;
            }
            SystemFunction oFunction = SystemFunction.Get(nId);
            if (null != oFunction)
            {
                if (oFunction.ParentId == -1)
                {
                    PageUtil.PageAlert(this.Page, "不能修改模块信息！");
                    PageUtil.PageAppendScript(this.Page, "top.windowFactory.closeTopFocusForm();");
                    return;
                }
                PageUtil.PageFillEdit(this, oFunction);
                txt_Key.Disabled = true;
                if (!string.IsNullOrEmpty(oFunction.IconName))
                {
                    img_IconName.Src = SystemUtil.ResovleActionImagePath(oFunction.IconName);
                    img_IconName.Attributes["title"] = oFunction.IconName;
                }
            }
            SystemFunction parentFunction = SystemFunction.Get(null != oFunction ? oFunction.ParentId : nParentId);
            lbl_ParentNmae.Text = null != parentFunction ? parentFunction.Name : "根节点";
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strName = txt_Name.Value.Trim();
            string strKey = txt_Key.Value.Trim();
            SystemFunction oFunction = SystemFunction.Get(nId);
            if (null == oFunction)
            {
                oFunction = new SystemFunction();
                oFunction.ModuleId = nParentId == -1 ? -1 : SystemFunction.Get(nParentId).ModuleId;
                oFunction.ParentId = nParentId;
                oFunction.Level = nParentId == -1 ? 1 : (SystemFunction.Get(nParentId).Level + 1);
            }
            PageUtil.PageFillEntity(this, oFunction);
            bool bSuccess = SystemFunction.Save(oFunction) > 0;
            PageUtil.PageAlert(this.Page, bSuccess ? "保存成功！" : "保存失败！");
        }
    }
}