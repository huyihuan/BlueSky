using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using System.Collections;
using WebSystemBase.SystemClass;
using WebSystemBase.Utilities;
using BlueSky.Utilities;
using BlueSky.EntityAccess;

namespace WebWorld.SystemManage
{
    public partial class SetRoleFunction : System.Web.UI.UserControl
    {
        int nRoleId = -1;
        int nFunctionId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nRoleId = PageUtil.GetQueryInt(this.Request, "id", -1);
            nFunctionId = PageUtil.GetQueryInt(this.Request, "fn", -1);
            if (IsPostBack)
                return;
            _initForm();
        }

        Hashtable htExistIds = new Hashtable();
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
            if (nRoleId == -1)
                return;
            htExistIds.Clear();
            SystemRoleFunctionPermission[] alExistFunctions = SystemRoleFunctionPermission.GetRoleFunctions(nRoleId);
            if (null == alExistFunctions || alExistFunctions.Length == 0)
                return;
            foreach (SystemRoleFunctionPermission ot in alExistFunctions)
                htExistIds[ot.FunctionId] = ot.Id;
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
                    cbSelect.Checked = htExistIds.ContainsKey(oItem.Id);
                }

                Label lbl = e.Item.FindControl("lbl_OrderId") as Label;
                lbl.Text = e.Item.ItemIndex + 1 + "";

                lbl = e.Item.FindControl("lbl_FunctionName") as Label;
                lbl.Text = oItem.ParentId == -1 ? string.Format("<b>{0}</b>", oItem.Name) : string.Format("{0}{1}", " ".PadLeft((oItem.Level - 1) * 4, '　'), htExistIds.ContainsKey(oItem.Id) ? string.Format("<a href='javascript:selectAction(\"{0}\",\"{1}\");' onclick='Utils.eventPrevent();'>{1}</a>", SystemUtil.ResovleSingleFormUrl(nFunctionId, "SetRoleAction", string.Format("setfn={0}&roleid={1}", oItem.Id, nRoleId)), oItem.Name) : oItem.Name);
                
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (nRoleId == -1)
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
                if (!cbSelect.Checked && htExistIds.ContainsKey(nId))
                {
                    SystemRoleFunctionPermission.Delete((int)htExistIds[nId]);
                }
                else if(cbSelect.Checked && !htExistIds.ContainsKey(nId))
                {
                    SystemRoleFunctionPermission addItem = new SystemRoleFunctionPermission();
                    addItem.RoleId = nRoleId;
                    addItem.FunctionId = nId;
                    HEntityCommon.HEntity(addItem).EntitySave();
                }
            }
            PageUtil.PageAlert(this.Page, "保存成功！");
            _initForm();
        }
    }
}