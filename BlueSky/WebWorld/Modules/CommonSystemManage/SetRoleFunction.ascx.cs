using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataBase;
using System.Web.UI.HtmlControls;
using System.Collections;
using WebWorld.Modules.CommonSystemManage.Class;

namespace WebWorld.Modules.CommonSystemManage
{
    public partial class SetRoleFunction : System.Web.UI.UserControl
    {
        int nRoleId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            nRoleId = PageUtil.GetQueryInt(this.Request, "roleid", -1);
            if (IsPostBack)
                return;
            _initForm();
        }

        Hashtable htExistIds = new Hashtable();
        FunctionItem[] AllFunctions;
        public void _initForm()
        {
            _initExistFunctions();
            AllFunctions = FunctionItem.GetFunctions(-1, true);
            FunctionItem[] rootFunctions = FunctionItem.GetFunctions(-1, false);
            List<FunctionItem> ltItems = new List<FunctionItem>();
            foreach (FunctionItem rootFunction in rootFunctions)
            {
                ltItems.Add(rootFunction);
                ltItems.AddRange(_GetSon(rootFunction));
            }
            rptItems.DataSource = ltItems.ToArray();
            rptItems.DataBind();
        }

        private FunctionItem[] _GetSon(FunctionItem _parentFunction)
        {
            List<FunctionItem> ltSon = new List<FunctionItem>();
            foreach (FunctionItem son in AllFunctions)
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
            RoleFunctionItem[] alExistFunctions = RoleFunctionItem.GetRoleFunctions(nRoleId);
            foreach (RoleFunctionItem ot in alExistFunctions)
            {
                htExistIds[ot.FunctionItemId] = ot.Id;
            }
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FunctionItem oItem = (FunctionItem)e.Item.DataItem;
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
                lbl.Text = oItem.ParentId == -1 ? string.Format("<b>{0}</b>", oItem.Name) : string.Format("{0}{1}", " ".PadLeft((oItem.Level - 1) * 4,'　'), oItem.Name);
                
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
                int nId = Util.ParseInt(cbSelect.Value, -1);
                if (nId == -1)
                    continue;
                if (!cbSelect.Checked && htExistIds.ContainsKey(nId))
                {
                    RoleFunctionItem.Delete((int)htExistIds[nId]);
                }
                else if(cbSelect.Checked && !htExistIds.ContainsKey(nId))
                {
                    RoleFunctionItem addItem = new RoleFunctionItem();
                    addItem.RoleItemId = nRoleId;
                    addItem.FunctionItemId = nId;
                    HEntityCommon.HEntity(addItem).EntitySave();
                }
            }
            PageUtil.PageAlert(this.Page, "保存成功！");
            _initForm();
        }
    }
}