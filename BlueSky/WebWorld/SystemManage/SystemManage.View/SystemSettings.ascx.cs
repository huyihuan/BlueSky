using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueSky.DataAccess;
using System.Web.Configuration;
using System.Configuration;
using WebBase.Config;
using BlueSky.Utilities;
using System.Collections;
using System.Web.UI.HtmlControls;
using BlueSky.Cache;
using BlueSky.Interfaces;

namespace WebWorld.SystemManage
{
    public partial class SystemSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _InitDebuggingTab();
            }
        }

        protected void btnGo_ServerClick(object sender, EventArgs e)
        {
            string strSql = txt_OperSql.Value.Trim();
            if (string.IsNullOrEmpty(strSql))
                return;
            IDbSession dbSession = new SqlServerSession();
            int nSuccess = dbSession.Execute(strSql);
            lit_Message.Text = nSuccess >= 1 ? "Success!" : "Fail!";
        }

        protected void btn_Cancel_ServerClick(object sender, EventArgs e)
        {

        }

        private void _InitDebuggingTab()
        {
            ConfigAccess config = new ConfigAccess();
            string strIsEnable = config.GetAppSetting(ConfigAccess.APPSETTINGKEY_EnableDebuggingOn);
            cb_EnableDebuggingOn.Checked = strIsEnable == Constants.Yes.ToString();
            if (cb_EnableDebuggingOn.Checked)
            {
                txt_DebuggUserName.Value = config.GetAppSetting(ConfigAccess.APPSETTINGKEY_DebuggingUserName);
                txt_DebuggPassword.Value = config.GetAppSetting(ConfigAccess.APPSETTINGKEY_DebuggingPassword);
            }
            txt_DebuggUserName.Disabled = !cb_EnableDebuggingOn.Checked;
            txt_DebuggPassword.Disabled = !cb_EnableDebuggingOn.Checked;
        }

        protected void btn_DebuggingSave_Click(object sender, EventArgs e)
        {
            ConfigAccess config = new ConfigAccess();
            config.SetAppSetting(ConfigAccess.APPSETTINGKEY_EnableDebuggingOn, (cb_EnableDebuggingOn.Checked ? Constants.Yes : Constants.No).ToString());
            if (cb_EnableDebuggingOn.Checked)
            {
                config.SetAppSetting(ConfigAccess.APPSETTINGKEY_DebuggingUserName, txt_DebuggUserName.Value.Trim());
                config.SetAppSetting(ConfigAccess.APPSETTINGKEY_DebuggingPassword, txt_DebuggPassword.Value.Trim());
            }
            txt_DebuggUserName.Disabled = !cb_EnableDebuggingOn.Checked;
            txt_DebuggPassword.Disabled = !cb_EnableDebuggingOn.Checked;
        }
        protected void btn_CacheMoniter_Click(object sender, EventArgs e)
        {
            _InitEntityCache();
        }
        private void _InitEntityCache()
        {
            List<Hashtable> ltCache = new List<Hashtable>();
            Hashtable htEntityCache = CacheInfomation.EntityCacheInformation();
            if (null != htEntityCache && htEntityCache.Count >= 1)
            {
                foreach (string strKey in htEntityCache.Keys)
                {
                    Hashtable htCache = new Hashtable();
                    htCache.Add("EntityName", strKey);
                    htCache.Add("CacheCount", htEntityCache[strKey]);
                    htCache.Add("Type", "Entity");
                    ltCache.Add(htCache);
                }
            }
            Hashtable htListCache = CacheInfomation.ListCacheInformation();
            if (null != htListCache && htListCache.Count >= 1)
            {
                foreach (string strKey in htListCache.Keys)
                {
                    Hashtable htCache = new Hashtable();
                    htCache.Add("EntityName", strKey);
                    htCache.Add("CacheCount", htListCache[strKey]);
                    htCache.Add("Type", "List");
                    ltCache.Add(htCache);
                }
            }
            
            //lt_CacheInformation.Text = string.Format("缓存大小：{0} Kb（实体{1},列表{2}）。");
            rptItems.DataSource = ltCache;
            rptItems.DataBind();
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Hashtable htItem = (Hashtable)e.Item.DataItem;
                if (null == htItem)
                    return;
                HtmlInputCheckBox cbSelect = e.Item.FindControl("cbSelect") as HtmlInputCheckBox;
                if (null != cbSelect)
                {
                    cbSelect.Value = htItem["EntityName"] + "";
                }

                Literal lt = e.Item.FindControl("lt_OrderId") as Literal;
                lt.Text = e.Item.ItemIndex + 1 + "";

                lt = e.Item.FindControl("lt_Type") as Literal;
                lt.Text = htItem["Type"] + "";

                lt = e.Item.FindControl("lt_EntityName") as Literal;
                lt.Text = htItem["EntityName"] + "";

                lt = e.Item.FindControl("lt_CacheCount") as Literal;
                lt.Text = htItem["CacheCount"] + "";
            }
        }

        protected void btn_ClearCache_Click(object sender, EventArgs e)
        {
            CacheInfomation.ClearCache();
        }

        protected void btn_ClearEntity_ServerClick(object sender, EventArgs e)
        {
            CacheInfomation.ClearEntityCache();
        }

        protected void btn_ClearList_ServerClick(object sender, EventArgs e)
        {
            CacheInfomation.ClearListCache();
        }

    }
}