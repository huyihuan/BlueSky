using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;
using WebBase.Utilities;
using System.Collections;
using System.Web.UI.HtmlControls;

using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class SetUserFunction : System.Web.UI.UserControl
    {
        int nUserId = -1;
        int nFunctionId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nUserId = PageUtil.GetQueryInt(this.Request, "id", -1);
            nFunctionId = PageUtil.GetQueryInt(this.Request, "fn", -1);
            if (IsPostBack)
                return;
            _initForm();
        }

        Hashtable htExistUserFn = new Hashtable();
        Hashtable htExistRoleFn = new Hashtable();
        SystemFunction[] AllFunctions;
        public void _initForm()
        {
            _initExistFunctions();
            //
            AllFunctions = SystemFunction.GetFunctions(-1, true);
            SystemFunction[] rootFunctions = SystemFunction.GetFunctions(-1, false);
            List<SystemFunction> ltItems = new List<SystemFunction>();
            foreach (SystemFunction rootFunction in rootFunctions)
            {
                ltItems.Add(rootFunction);
                ltItems.AddRange(_GetSon(rootFunction));
            }
            rptItems.DataSource = ltItems.ToArray();
            rptItems.DataBind();
        }

        private SystemFunction[] _GetSon(SystemFunction _parentFunction)
        {
            List<SystemFunction> ltSon = new List<SystemFunction>();
            foreach (SystemFunction son in AllFunctions)
            {
                if (son.ParentId == _parentFunction.Id)
                {
                    ltSon.Add(son);
                    ltSon.AddRange(_GetSon(son));
                }
            }
            return ltSon.ToArray();
        }

        private void _initExistFunctions()
        {
            if (nUserId == -1)
                return;
            htExistRoleFn.Clear();
            htExistUserFn.Clear();
            SystemUserRole[] alUserRoles = SystemUserRole.GetUserRoles(nUserId);
            if (null != alUserRoles && alUserRoles.Length > 0)
            {
                foreach (SystemUserRole item in alUserRoles)
                {
                    SystemRoleFunctionPermission[] alExistRoleFns = SystemRoleFunctionPermission.GetRoleFunctions(item.RoleId);
                    if (null == alExistRoleFns || alExistRoleFns.Length == 0)
                        continue;
                    foreach (SystemRoleFunctionPermission ot in alExistRoleFns)
                        htExistRoleFn[ot.FunctionId] = ot.Id;
                }
            }

            SystemUserFunctionPermission[] alExistUserFns = SystemUserFunctionPermission.GetUserFunctionPermission(nUserId);
            if (null != alExistUserFns && alExistUserFns.Length > 0)
            {
                foreach (SystemUserFunctionPermission item in alExistUserFns)
                    htExistUserFn[item.FunctionId] = item.Id;
            }
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SystemFunction oItem = (SystemFunction)e.Item.DataItem;
                if (null == oItem)
                    return;
                PageUtil.PageFillView(e.Item, oItem);

                HtmlInputCheckBox cbSelect = e.Item.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null != cbSelect)
                {
                    cbSelect.Value = oItem.Id + "";
                    cbSelect.Checked = htExistRoleFn.ContainsKey(oItem.Id) || htExistUserFn.ContainsKey(oItem.Id);
                    cbSelect.Disabled = htExistRoleFn.ContainsKey(oItem.Id);
                    cbSelect.Attributes["title"] = htExistRoleFn.ContainsKey(oItem.Id) ? "继承自角色，不能修改！" : "";
                }

                Label lbl = e.Item.FindControl("lbl_OrderId") as Label;
                if (null != lbl)
                    lbl.Text = e.Item.ItemIndex + 1 + "";

                lbl = e.Item.FindControl("lbl_FunctionName") as Label;
                if (null != lbl)
                    lbl.Text = oItem.ParentId == -1 ? string.Format("<b>{0}</b>", oItem.Name) : string.Format("{0}{1}", " ".PadLeft((oItem.Level - 1) * 4, '　'), cbSelect.Checked ? string.Format("<a href='javascript:selectAction(\"{0}\",\"{1}\");'>{1}</a>", string.Format("{0}&userid={1}&setfn={2}", SystemUtil.ResovleSingleFormUrl(nFunctionId, "SetUserAction"), nUserId, oItem.Id), oItem.Name) : oItem.Name);

                lbl = e.Item.FindControl("lbl_Remark") as Label;
                if (null != lbl)
                    lbl.Text = htExistRoleFn.ContainsKey(oItem.Id) ? "继承自角色" : "--";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (nUserId == -1)
                return;
            _initExistFunctions();
            foreach (RepeaterItem oItem in rptItems.Items)
            {
                HtmlInputCheckBox cbSelect = oItem.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null == cbSelect)
                    continue;
                int nId = TypeUtil.ParseInt(cbSelect.Value, -1);
                if (nId == -1)
                    continue;
                //尽量避免SystemUserFunctionPermission和表SystemRoleFunctionPermission数据产生交集
                if ((!cbSelect.Checked || cbSelect.Disabled) && htExistUserFn.ContainsKey(nId))
                {
                    SystemUserFunctionPermission.Delete((int)htExistUserFn[nId]);
                }
                else if (!cbSelect.Disabled && cbSelect.Checked && !htExistUserFn.ContainsKey(nId))
                {
                    SystemUserFunctionPermission addItem = new SystemUserFunctionPermission();
                    addItem.UserId = nUserId;
                    addItem.FunctionId = nId;
                    SystemUserFunctionPermission.Save(addItem);
                }
            }
            PageUtil.PageAlert(this.Page, "保存成功！");
            _initForm();
        }
    }
}