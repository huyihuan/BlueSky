﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataBase;
using System.Collections;
using WebWorld.Modules.CommonSystemManage.Class;

namespace WebWorld.Modules.CommonSystemManage
{
    public partial class Login : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strUserName = txt_UserName.Value.Trim();
            string strPassword = txt_Password.Value;
            string strVCode = txt_VCode.Value;
            if ("" == strUserName || "" == strPassword || "" == strVCode)
                return;
            if (!strVCode.ToLower().Equals(DataBase.Util.VCodeGetCurrent().ToLower()))
            {
                PageUtil.PageAlert(this.Page, "验证码错误！");
                return;
            }
            
            if (!UserItem.ExistUser(strUserName, strPassword))
            {
                PageUtil.PageAlert(this.Page, "用户名或密码错误！");
                return;
            }
            SaveLoginUserItem(strUserName);
            PageUtil.PageAppendScript(this.Page, "top.window.location.href = \"Index.aspx\";");
        }

        private void SaveLoginUserItem(string _strUserName)
        {
            UserItem loginItem = UserItem.Get(_strUserName);
            if (null == loginItem)
                return;
            Hashtable htUserInfomation = new Hashtable();
            htUserInfomation["UserName"] = loginItem.UserName;
            htUserInfomation["Id"] = loginItem.Id;
            SystemUtil.SaveLoginUser(htUserInfomation);
        }
    }
}