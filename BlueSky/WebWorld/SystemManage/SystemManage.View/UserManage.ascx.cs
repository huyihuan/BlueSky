using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.SystemClass;
using WebBase.Utilities;

using BlueSky.EntityAccess;
using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class UserManage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbl_Message.InnerHtml = string.Format("当前系统用户数量 <b>{0}</b> 个。", EntityAccess<UserInformation>.Access.Count());
                SystemRole.BindList(sel_Role, true, false, true);
            }
        }

        protected void btn_Add_ServerClick(object sender, EventArgs e)
        {
            int nAdd = TypeUtil.ParseInt(txt_AddNumber.Value, 0);
            if (nAdd <= 0)
            {
                PageUtil.PageAlert(this.Page, "请填写要新增的用户数量");
                return;
            }
            Server.ScriptTimeout = nAdd;
            int nId = EntityAccess<UserInformation>.Access.Count();
            nAdd += nId;
            DateTime dtBegin = DateTime.Now;
            int nRoleId = TypeUtil.ParseInt(sel_Role.SelectedValue, -1);
            bool bSelectRole = nRoleId >= 1;
            for (int i = nId; i < nAdd; i++)
            {
                UserInformation oUser = new UserInformation();
                oUser.UserName = "huyihuan" + i;
                oUser.Password = "1";
                oUser.Age = 18;
                oUser.Gender = 1;
                oUser.NickName = "胡伊欢下属" + i;
                oUser.QQ = "814822671";
                oUser.PostCode = "471000";
                oUser.MSN = "5749230583";
                oUser.Email = "huyihuan@jzs.com.cn";
                oUser.CardID = "410489189508043674";
                int nAddId = UserInformation.Save(oUser);
                if (nAddId >= 1 && bSelectRole)
                {
                    SystemUserRole oUserRole = new SystemUserRole();
                    oUserRole.RoleId = nRoleId;
                    oUserRole.UserId = nAddId;
                    SystemUserRole.Save(oUserRole);
                }
            }
            lbl_Message.InnerHtml = string.Format("<br />新增完成，当前系统用户数量 <b>{0}</b> 个。", EntityAccess<UserInformation>.Access.Count());
            TimeSpan tsDuring = new TimeSpan(DateTime.Now.Ticks - dtBegin.Ticks);
            lbl_Message.InnerHtml += string.Format("<br />共用时 <b>{0}</b>s。", tsDuring.TotalSeconds);
        }

    }
}