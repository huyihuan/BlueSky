﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using DataBase;
using WebSystemBase.SystemClass;
using WebSystemBase.Utilities;

namespace WebWorld.SystemManage
{
    public partial class FunctionsManage : System.Web.UI.UserControl
    {
        public string strTree = "";
        public string strActionUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            strActionUrl = SystemUtil.ResovleActionFormURL("ActionList");
            if (IsPostBack)
                return;
            strTree = SystemFunctionUtil.CreateFunctionTree();
        }
    }
}