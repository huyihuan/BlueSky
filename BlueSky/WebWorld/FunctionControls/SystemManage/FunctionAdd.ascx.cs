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
    public partial class FunctionAdd : System.Web.UI.UserControl
    {
        int nId = -1, nParentId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            nParentId = PageUtil.GetQueryInt(this.Request, "parentid", -1);
            if (IsPostBack)
                return;
            FunctionUtil.BindTargetList(sel_Target);
            InitForm();
        }

        protected void InitForm()
        {
            FunctionItem parentFunction = FunctionItem.Get(nParentId);
            lbl_ParentNmae.Text = null != parentFunction ? parentFunction.Name : "根节点";
            if (nId >= 0)
            {
                FunctionItem childFunc = FunctionItem.Get(nId);
                if (null != childFunc)
                {
                    txt_FunctionName.Value = childFunc.Name;
                    txt_Value.Value = childFunc.Value;
                    txt_Tip.Value = childFunc.Tip;
                    txt_Width.Value = childFunc.Width + "";
                    txt_Height.Value = childFunc.Height + "";
                    txt_Image.Value = childFunc.IconName;
                    if (childFunc.ParentId != -1)
                    {
                        FunctionItem parent = FunctionItem.Get(childFunc.ParentId);
                        if (null != parent)
                            lbl_ParentNmae.Text = parent.Name;
                    }
                    else {
                        lbl_ParentNmae.Text = "根节点";
                    }
                    PageUtil.SetSelectValue(sel_Target, childFunc.Target);
                    rb_IsResizeYes.Checked = childFunc.IsResize == Constants.Yes;
                    rb_IsResizeNo.Checked = childFunc.IsResize == Constants.No;
                    rb_IsToMoveYes.Checked = childFunc.IsToMove == Constants.Yes;
                    rb_IsToMoveNo.Checked = childFunc.IsToMove == Constants.No;
                    rb_IsShowInTaskBarYes.Checked = childFunc.IsShowInTaskBar == Constants.Yes;
                    rb_IsShowInTaskBarNo.Checked = childFunc.IsShowInTaskBar == Constants.No;
                    rb_IsIncludeMinBoxYes.Checked = childFunc.IsIncludeMinBox == Constants.Yes;
                    rb_IsIncludeMinBoxNo.Checked = childFunc.IsIncludeMinBox == Constants.No;
                    rb_IsIncludeMaxBoxYes.Checked = childFunc.IsIncludeMaxBox == Constants.Yes;
                    rb_IsIncludeMaxBoxNo.Checked = childFunc.IsIncludeMaxBox == Constants.No;
                }   
            }
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strFuncName = txt_FunctionName.Value.Trim();
            string strValue = txt_Value.Value.Trim();
            string strTip = txt_Tip.Value.Trim();
            string strImage = txt_Image.Value.Trim();

            FunctionItem funcObj = FunctionItem.Get(nId);
            if (null == funcObj)
            {
                funcObj = new FunctionItem();
                funcObj.ParentId = nParentId;
                funcObj.Level = nParentId == -1 ? -1 : (FunctionItem.Get(nParentId).Level + 1);
            }

            funcObj.Name = strFuncName;
            funcObj.Value = strValue;
            funcObj.Tip = strTip;
            funcObj.IconName = strImage;
            funcObj.Target = sel_Target.SelectedValue;
            funcObj.Width = Util.ParseInt(txt_Width.Value.Trim(), 0);
            funcObj.Height = Util.ParseInt(txt_Height.Value.Trim(), 0);
            funcObj.IsResize = rb_IsResizeYes.Checked ? Constants.Yes : Constants.No;
            funcObj.IsToMove = rb_IsToMoveYes.Checked ? Constants.Yes : Constants.No;
            funcObj.IsShowInTaskBar = rb_IsShowInTaskBarYes.Checked ? Constants.Yes : Constants.No;
            funcObj.IsIncludeMinBox = rb_IsIncludeMinBoxYes.Checked ? Constants.Yes : Constants.No;
            funcObj.IsIncludeMaxBox = rb_IsIncludeMaxBoxYes.Checked ? Constants.Yes : Constants.No;
            HEntityCommon.HEntity(funcObj).EntitySave();
            PageUtil.PageAlert(this.Page, "保存成功！");
            PageUtil.PageAppendScript(this.Page, "top.windowFactory.refreshRoot();top.windowFactory.closeTopFocusForm();");
        }
    }
}