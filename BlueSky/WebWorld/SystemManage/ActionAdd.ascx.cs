using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;
using WebSystemBase.SystemClass;

namespace WebWorld.SystemManage
{
    public partial class ActionAdd : System.Web.UI.UserControl
    {
        int nId = -1, nFunctionId = -1;
        public string strImageFormUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            nFunctionId = PageUtil.GetQueryInt(this.Request, "setfn", -1);
            strImageFormUrl = SystemUtil.ResovleSingleFormUrl(this.Request, "ActionImage");
            if (IsPostBack)
                return;
            SystemAction.BindActionTypeList(sel_ActionType, true, false);
            InitForm();
        }

        private static string[] astrActionTypeNeedValue = new string[] { SystemAction.CONST_ACTIONTYPE_JAVASCRIPT, SystemAction.CONST_ACTIONTYPE_HTTP };
        protected void InitForm()
        {
            if (nFunctionId > 0)
            {
                SystemFunction oFunction = SystemFunction.Get(nFunctionId);
                lbl_FunctionNmae.Text = null != oFunction ? oFunction.Name : "";
            }
            if (nId >= 0)
            {
                SystemAction oAction = SystemAction.Get(nId);
                if (null != oAction)
                {
                    PageUtil.PageFillEdit(this, oAction);
                }
                txt_Key.Disabled = true;
                if (!string.IsNullOrEmpty(oAction.IconName))
                {
                    img_IconName.Src = SystemUtil.ResovleActionImagePath(oAction.IconName);
                    img_IconName.Attributes["title"] = oAction.IconName;
                }
                sel_ActionType_SelectedIndexChanaged(null, null);

                SystemFunction oFunction = SystemFunction.Get(oAction.FunctionId);
                lbl_FunctionNmae.Text = oFunction.Name;
            }
        }

        protected void sel_ActionType_SelectedIndexChanaged(object sender, EventArgs e)
        {
            txt_ActionValue.Visible = astrActionTypeNeedValue.Contains(sel_ActionType.SelectedValue);
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            if (nFunctionId <= 0)
            {
                PageUtil.PageAlert(this.Page, "请选择操作所属功能！");
                return;
            }
            string strActionName = txt_Name.Value.Trim();
            string strKey = txt_Key.Value.Trim();
            string strControlName = txt_ControlName.Value.Trim();
            if (string.IsNullOrEmpty(strActionName) || string.IsNullOrEmpty(strKey) || string.IsNullOrEmpty(strControlName))
                return;

            SystemAction oAction = SystemAction.Get(nId);
            if (null == oAction)
            {
                oAction = new SystemAction();
                oAction.FunctionId = nFunctionId;
            }
            PageUtil.PageFillEntity(this, oAction);
            SystemAction.Save(oAction);
            PageUtil.PageAlert(this.Page, "保存成功！");
        }
    }
}