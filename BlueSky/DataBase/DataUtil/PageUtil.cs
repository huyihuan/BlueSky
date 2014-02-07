using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace DataBase
{
    public class PageUtil
    {
        public static string GetQueryString(HttpRequest _httpRequest, string _strKey)
        {
            string strValue = _httpRequest.QueryString[_strKey] + "";
            return strValue;
        }

        public static int GetQueryInt(HttpRequest _httpRequest, string _strKey, int _nDefaultValue)
        {
            string strValue = _httpRequest.QueryString[_strKey] + "";
            return Util.ParseInt(strValue, _nDefaultValue);
        }

        public static void PageAlert(System.Web.UI.Page _Page, string _MessageContent)
        {
            if (null == _Page)
                return;
            _Page.ClientScript.RegisterClientScriptBlock(_Page.GetType(), "alert", string.Format("<script type=\"text/javascript\">alert(\"{0}\")</script>", _MessageContent));
        }

        public static void PageAppendScript(System.Web.UI.Page _Page, string _Script)
        {
            if (null == _Page)
                return;
            _Page.ClientScript.RegisterClientScriptBlock(_Page.GetType(), "script", string.Format("<script type=\"text/javascript\">{0}</script>", _Script));
        }

        public static void SetSelectValue(ListControl _ltControl, string _strSelValue)
        {
            if (null == _ltControl || _ltControl.Items.Count == 0)
                return;
            ListItem liExist = _ltControl.Items.FindByValue(_strSelValue);
            if (null != liExist)
                liExist.Selected = true;
        }

        public static void PageFillView(System.Web.UI.Page _Page,object _FillObj)
        {
            if (null == _Page || null == _FillObj)
                return;
            string[] astrFields = Util.GetObjectFieldsList(_FillObj);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strConrolId = "";
            Hashtable htFieldValue = Util.GetObjectFieldValueHash(_FillObj);
            foreach (string strFieldName in astrFields)
            {
                strConrolId = "lbl_" + strFieldName;
                Label lblControl = _Page.FindControl(strConrolId) as Label;
                if (null != lblControl)
                    lblControl.Text = htFieldValue[strFieldName] + "";
            }
        }

        public static void PageFillView(System.Web.UI.Control _ParentControl, object _FillObj)
        {
            if (null == _ParentControl || null == _FillObj)
                return;
            string[] astrFields = Util.GetObjectFieldsList(_FillObj);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strConrolId = "";
            Hashtable htFieldValue = Util.GetObjectFieldValueHash(_FillObj);
            foreach (string strFieldName in astrFields)
            {
                strConrolId = "lbl_" + strFieldName;
                Label lblControl = _ParentControl.FindControl(strConrolId) as Label;
                if (null != lblControl)
                    lblControl.Text = htFieldValue[strFieldName] + "";
            }
        }

        public static void PageFillEdit(System.Web.UI.Page _Page, object _FillObj)
        {
            if (null == _Page || null == _FillObj)
                return;
            string[] astrFields = Util.GetObjectFieldsList(_FillObj);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strControlId = "";
            Hashtable htFieldValue = Util.GetObjectFieldValueHash(_FillObj);
            foreach (string strFieldName in astrFields)
            {
                string strValue = htFieldValue[strFieldName] + "";

                strControlId = "txt_" + strFieldName;
                HtmlInputText txtControlHtml = _Page.FindControl(strControlId) as HtmlInputText;
                if (null != txtControlHtml)
                {
                    txtControlHtml.Value = strValue;
                    continue;
                }
                TextBox txtControl = _Page.FindControl(strControlId) as TextBox;
                if (null != txtControl)
                {
                    txtControl.Text = strValue;
                    continue;
                }
                HtmlTextArea areaControlHtml = _Page.FindControl(strControlId) as HtmlTextArea;
                if (null != areaControlHtml)
                {
                    areaControlHtml.Value = strValue;
                    continue;
                }

                strControlId = "sel_" + strFieldName;
                DropDownList ddlControl = _Page.FindControl(strControlId) as DropDownList;
                if (null != ddlControl)
                {
                    SetSelectValue(ddlControl, strValue);
                    continue;
                }
                HtmlSelect ddlControlHtml = _Page.FindControl(strControlId) as HtmlSelect;
                if (null != ddlControlHtml)
                {
                    ListItem Li = ddlControlHtml.Items.FindByValue(strValue);
                    if (null != Li)
                        Li.Selected = true;
                    continue;
                }
            }
        }

        public static void PageFillEdit(System.Web.UI.Control _ParentControl, object _FillObj)
        {
            if (null == _ParentControl || null == _FillObj)
                return;
            string[] astrFields = Util.GetObjectFieldsList(_FillObj);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strControlId = "";
            Hashtable htFieldValue = Util.GetObjectFieldValueHash(_FillObj);
            foreach (string strFieldName in astrFields)
            {
                string strValue = htFieldValue[strFieldName] + "";

                strControlId = "txt_" + strFieldName;
                HtmlInputText txtControlHtml = _ParentControl.FindControl(strControlId) as HtmlInputText;
                if (null != txtControlHtml)
                {
                    txtControlHtml.Value = strValue;
                    continue;
                }
                TextBox txtControl = _ParentControl.FindControl(strControlId) as TextBox;
                if (null != txtControl)
                {
                    txtControl.Text = strValue;
                    continue;
                }
                HtmlTextArea areaControlHtml = _ParentControl.FindControl(strControlId) as HtmlTextArea;
                if (null != areaControlHtml)
                {
                    areaControlHtml.Value = strValue;
                    continue;
                }

                strControlId = "sel_" + strFieldName;
                DropDownList ddlControl = _ParentControl.FindControl(strControlId) as DropDownList;
                if (null != ddlControl)
                {
                    SetSelectValue(ddlControl, strValue);
                    continue;
                }
                HtmlSelect ddlControlHtml = _ParentControl.FindControl(strControlId) as HtmlSelect;
                if (null != ddlControlHtml)
                {
                    ListItem Li = ddlControlHtml.Items.FindByValue(strValue);
                    if (null != Li)
                        Li.Selected = true;
                    continue;
                }
            }
        }
    }
}
