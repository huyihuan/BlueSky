using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSystemBase.SystemClass;
using DataBase;
using WebSystemBase.Utilities;

namespace WebWorld.SystemManage
{
    public partial class ModuleAdd : System.Web.UI.UserControl
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
            SystemModule oGet = SystemModule.Get(nId);
            if (null != oGet)
            {
                PageUtil.PageFillEdit(this, oGet);
                txt_Key.Disabled = true;
            }
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            string strModuleName = txt_Name.Value.Trim();
            string strKey = txt_Key.Value.Trim();
            string strController = txt_Controller.Value.Trim();
            string strDescription = txt_Description.Text.Trim();
            if ("" == strModuleName || "" == strKey || "" == strController)
                return;
            if (nId <= 0 && SystemModule.Exist(strKey))
            {
                PageUtil.PageAlert(this.Page, "该模块Key已存在！");
                return;
            }
            SystemModule addItem = SystemModule.Get(nId);
            if (null == addItem)
            {
                addItem = new SystemModule();
                addItem.Key = strKey;
            }
            addItem.Name = strModuleName;
            addItem.Controller = strController;
            addItem.Description = strDescription;
            int nNewId = SystemModule.Save(addItem);
            if (nNewId <= 0)
                return;
            PageUtil.PageAlert(this.Page, "保存成功！");
        }
    }
}