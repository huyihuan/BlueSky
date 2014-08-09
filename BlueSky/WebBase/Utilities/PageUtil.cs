﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Reflection;
using BlueSky.Utilities;
using BlueSky.Interfaces;
using BlueSky.EntityAccess;

namespace WebBase.Utilities
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
            return TypeUtil.ParseInt(strValue, _nDefaultValue);
        }
        public static int GetQueryId(HttpRequest _httpRequest, int _nDefaultValue)
        { 
            return GetQueryInt(_httpRequest, "id",_nDefaultValue);
        }
        public static string GetQueryId(HttpRequest _httpRequest)
        { 
            return GetQueryString(_httpRequest, "id");
        }
        public static string GetQueryIds(HttpRequest _httpRequest)
        {
            return GetQueryString(_httpRequest, "ids");
        }
        public static string[] GetQueryArrayIds(HttpRequest _httpRequest)
        {
            return GetQueryIds(_httpRequest).Split(new char[] { ';'});
        }
        public static int[] GetQueryArrayIds(HttpRequest _httpRequest, int _nDefaultValue)
        {
            string[] al = GetQueryArrayIds(_httpRequest);
            List<int> lt = new List<int>();
            if (null != al && al.Length > 0)
            {
                foreach (string s in al)
                {
                    lt.Add(TypeUtil.ParseInt(s, _nDefaultValue));
                }
            }
            return lt.ToArray();
        }
        public static void PageSetExtraParameters(System.Web.UI.Page _Page, string _strParameters)
        {
            HtmlInputHidden hiddenParameters = _Page.FindControl("hiddenExtraParameters") as HtmlInputHidden;
            if (null == hiddenParameters)
                return;
            if (_strParameters.StartsWith("&"))
                _strParameters = _strParameters.Remove(0, 1);
            hiddenParameters.Value = _strParameters;
        }
        public static void PageAlert(System.Web.UI.Page _Page, string _MessageContent)
        {
            if (null == _Page)
                return;
            _Page.ClientScript.RegisterClientScriptBlock(_Page.GetType(), "alert", string.Format("alert('{0}')", _MessageContent), true);
        }
        public static void PageAppendScript(System.Web.UI.Page _Page, string _Script)
        {
            if (null == _Page)
                return;
            _Page.ClientScript.RegisterClientScriptBlock(_Page.GetType(), "script", string.Format("<script type=\"text/javascript\">{0}</script>", _Script));
        }
        public static void PageRefreshLayout(System.Web.UI.Page _Page)
        {
            PageAppendScript(_Page, "top.window.location.href = top.window.location.href;");
        }
        public static void PageRefreshActiveWindow(System.Web.UI.Page _Page)
        {
            PageAppendScript(_Page, "top.layout.refreshActiveWindow();");
        }
        public static void PageClosePopupWindow(System.Web.UI.Page _Page)
        {
            PageClosePopupWindow(_Page, false);
        }
        public static void PageClosePopupWindow(System.Web.UI.Page _Page, bool _isRefreshParentWindow)
        {
            if (_isRefreshParentWindow)
                PageRefreshActiveWindow(_Page);
            PageAppendScript(_Page, "top.windowFactory.closeTopFocusForm();");
        }
        public static object PageSelectHiddenValue(System.Web.UI.Page _Page,bool _bNeedArray)
        {
            HtmlInputHidden hiddenSelectedValue = _Page.FindControl("hiddenSelectedValue") as HtmlInputHidden;
            if (null == hiddenSelectedValue || string.IsNullOrEmpty(hiddenSelectedValue.Value))
                return null;
            string strValue = hiddenSelectedValue.Value.Trim();
            if (_bNeedArray)
                return strValue.Split(new char[] { ',' });
            return strValue;
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
            string[] astrFields = ReflectionUtil.GetObjectFieldsList(_FillObj, true, false);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strConrolId = "";
            Hashtable htFieldValue = ReflectionUtil.GetObjectFieldValueHash(_FillObj);
            foreach (string strFieldName in astrFields)
            {
                strConrolId = "lbl_" + strFieldName;
                Label lblControl = _Page.FindControl(strConrolId) as Label;
                if (null != lblControl)
                {
                    lblControl.Text = htFieldValue[strFieldName] + "";
                    continue;
                }

                strConrolId = "lit_" + strFieldName;
                Literal litControl = _Page.FindControl(strConrolId) as Literal;
                if (null != litControl)
                {
                    litControl.Text = htFieldValue[strFieldName] + "";
                    continue;
                }
            }
        }
        public static void PageFillView(System.Web.UI.Control _ParentControl, object _FillObj)
        {
            if (null == _ParentControl || null == _FillObj)
                return;
            string[] astrFields = ReflectionUtil.GetObjectFieldsList(_FillObj, true, false);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strConrolId = "";
            Hashtable htFieldValue = ReflectionUtil.GetObjectFieldValueHash(_FillObj);
            foreach (string strFieldName in astrFields)
            {
                strConrolId = "lbl_" + strFieldName;
                Label lblControl = _ParentControl.FindControl(strConrolId) as Label;
                if (null != lblControl)
                {
                    lblControl.Text = htFieldValue[strFieldName] + "";
                    continue;
                }

                strConrolId = "lit_" + strFieldName;
                Literal litControl = _ParentControl.FindControl(strConrolId) as Literal;
                if (null != litControl)
                {
                    litControl.Text = htFieldValue[strFieldName] + "";
                    continue;
                }
            }
        }
        public static void PageFillEdit(System.Web.UI.Page _Page, object _FillObj)
        {
            if (null == _Page || null == _FillObj)
                return;
            string[] astrFields = ReflectionUtil.GetObjectFieldsList(_FillObj, true, false);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strControlId = "";
            Hashtable htFieldValue = ReflectionUtil.GetObjectFieldValueHash(_FillObj);
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

                HtmlInputPassword txtPassword = _Page.FindControl(strControlId) as HtmlInputPassword;
                if(null != txtPassword)
                {
                    txtPassword.Value = strValue;
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

                strControlId = "cb_" + strFieldName;
                HtmlInputCheckBox cbControlHtml = _Page.FindControl(strControlId) as HtmlInputCheckBox;
                if (null != cbControlHtml)
                {
                    cbControlHtml.Checked = TypeUtil.ParseInt(strValue, -1) == Constants.Yes;
                    continue;
                }
                CheckBox cbControl = _Page.FindControl(strControlId) as CheckBox;
                if (null != cbControl)
                {
                    cbControl.Checked = TypeUtil.ParseInt(strValue, -1) == Constants.Yes;
                    continue;
                }
            }
        }
        public static void PageFillEdit(System.Web.UI.Control _ParentControl, object _FillObj)
        {
            if (null == _ParentControl || null == _FillObj)
                return;
            string[] astrFields = ReflectionUtil.GetObjectFieldsList(_FillObj, true, false);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strControlId = "";
            Hashtable htFieldValue = ReflectionUtil.GetObjectFieldValueHash(_FillObj);
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

                HtmlInputPassword txtPassword = _ParentControl.FindControl(strControlId) as HtmlInputPassword;
                if (null != txtPassword)
                {
                    txtPassword.Value = strValue;
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

                strControlId = "cb_" + strFieldName;
                HtmlInputCheckBox cbControlHtml = _ParentControl.FindControl(strControlId) as HtmlInputCheckBox;
                if (null != cbControlHtml)
                {
                    cbControlHtml.Checked = TypeUtil.ParseInt(strValue, -1) == Constants.Yes;
                    continue;
                }
                CheckBox cbControl = _ParentControl.FindControl(strControlId) as CheckBox;
                if (null != cbControl)
                {
                    cbControl.Checked = TypeUtil.ParseInt(strValue, -1) == Constants.Yes;
                    continue;
                }
            }
        }
        public static void PageFillEntity(System.Web.UI.Control _ParentControl, object _oE)
        {
            if (null == _ParentControl || null == _oE)
                return;
            string[] astrFields = ReflectionUtil.GetObjectFieldsList(_oE, true, true);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strControlId = "", strValue = "";
            foreach (string strFieldName in astrFields)
            {
                strControlId = "txt_" + strFieldName;
                HtmlInputText txtControlHtml = _ParentControl.FindControl(strControlId) as HtmlInputText;
                if (null != txtControlHtml)
                {
                    if (txtControlHtml.Disabled)
                        continue;
                    strValue = txtControlHtml.Value.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                TextBox txtControl = _ParentControl.FindControl(strControlId) as TextBox;
                if (null != txtControl)
                {
                    if (!txtControl.Enabled)
                        continue;
                    strValue = txtControl.Text.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlTextArea areaControlHtml = _ParentControl.FindControl(strControlId) as HtmlTextArea;
                if (null != areaControlHtml)
                {
                    strValue = areaControlHtml.Value.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlInputPassword txtPassword = _ParentControl.FindControl(strControlId) as HtmlInputPassword;
                if (null != txtPassword)
                {
                    if (txtPassword.Disabled)
                        continue;
                    strValue = txtPassword.Value.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

                strControlId = "sel_" + strFieldName;
                DropDownList ddlControl = _ParentControl.FindControl(strControlId) as DropDownList;
                if (null != ddlControl)
                {
                    if (!ddlControl.Enabled)
                        continue;
                    strValue = ddlControl.SelectedValue.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlSelect ddlControlHtml = _ParentControl.FindControl(strControlId) as HtmlSelect;
                if (null != ddlControlHtml)
                {
                    if (ddlControlHtml.Disabled)
                        continue;
                    ListItem LiSelected = ddlControlHtml.Items[ddlControlHtml.SelectedIndex];
                    if (null == LiSelected)
                        continue;
                    strValue = LiSelected.Value.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

                strControlId = "cb_" + strFieldName;
                HtmlInputCheckBox cbControlHtml = _ParentControl.FindControl(strControlId) as HtmlInputCheckBox;
                if (null != cbControlHtml)
                {
                    if (cbControlHtml.Disabled)
                        continue;
                    strValue = (cbControlHtml.Checked ? Constants.Yes : Constants.No) + "";
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                CheckBox cbControl = _ParentControl.FindControl(strControlId) as CheckBox;
                if (null != cbControl)
                {
                    if (!cbControl.Enabled)
                        continue;
                    strValue = (cbControl.Checked ? Constants.Yes : Constants.No) + "";
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

            }
        }
        public static void PageFillEntity(System.Web.UI.Page _Page, object _oE)
        {
            if (null == _Page || null == _oE)
                return;
            string[] astrFields = ReflectionUtil.GetObjectFieldsList(_oE, true, true);
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strControlId = "", strValue = "";
            foreach (string strFieldName in astrFields)
            {
                strControlId = "txt_" + strFieldName;
                HtmlInputText txtControlHtml = _Page.FindControl(strControlId) as HtmlInputText;
                if (null != txtControlHtml)
                {
                    if (txtControlHtml.Disabled)
                        continue;
                    strValue = txtControlHtml.Value.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                TextBox txtControl = _Page.FindControl(strControlId) as TextBox;
                if (null != txtControl)
                {
                    if (!txtControl.Enabled)
                        continue;
                    strValue = txtControl.Text.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlTextArea areaControlHtml = _Page.FindControl(strControlId) as HtmlTextArea;
                if (null != areaControlHtml)
                {
                    strValue = areaControlHtml.Value.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlInputPassword txtPassword = _Page.FindControl(strControlId) as HtmlInputPassword;
                if (null != txtPassword)
                {
                    if (txtPassword.Disabled)
                        continue;
                    strValue = txtPassword.Value.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

                strControlId = "sel_" + strFieldName;
                DropDownList ddlControl = _Page.FindControl(strControlId) as DropDownList;
                if (null != ddlControl)
                {
                    if (!ddlControl.Enabled)
                        continue;
                    strValue = ddlControl.SelectedValue.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlSelect ddlControlHtml = _Page.FindControl(strControlId) as HtmlSelect;
                if (null != ddlControlHtml)
                {
                    if (ddlControlHtml.Disabled)
                        continue;
                    ListItem LiSelected = ddlControlHtml.Items[ddlControlHtml.SelectedIndex];
                    if (null == LiSelected)
                        continue;
                    strValue = LiSelected.Value.Trim();
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

                strControlId = "cb_" + strFieldName;
                HtmlInputCheckBox cbControlHtml = _Page.FindControl(strControlId) as HtmlInputCheckBox;
                if (null != cbControlHtml)
                {
                    if (cbControlHtml.Disabled)
                        continue;
                    strValue = (cbControlHtml.Checked ? Constants.Yes : Constants.No) + "";
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                CheckBox cbControl = _Page.FindControl(strControlId) as CheckBox;
                if (null != cbControl)
                {
                    if (!cbControl.Enabled)
                        continue;
                    strValue = (cbControl.Checked ? Constants.Yes : Constants.No) + "";
                    ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

            }
        }
        public static void PageFillEntity<TEntity>(System.Web.UI.Control _ParentControl, TEntity _oE) where TEntity : IEntity,new()
        {
            if (null == _ParentControl || null == _oE)
                return;
            IEntityField[] astrFields = EntityAccess<TEntity>.Meta.EntityFields;
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strControlId = "", strValue = "";
            foreach (IEntityField iEF in astrFields)
            {
                string strFieldName = iEF.FieldName;
                strControlId = "txt_" + strFieldName;
                HtmlInputText txtControlHtml = _ParentControl.FindControl(strControlId) as HtmlInputText;
                if (null != txtControlHtml)
                {
                    if (txtControlHtml.Disabled)
                        continue;
                    strValue = txtControlHtml.Value.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                TextBox txtControl = _ParentControl.FindControl(strControlId) as TextBox;
                if (null != txtControl)
                {
                    if (!txtControl.Enabled)
                        continue;
                    strValue = txtControl.Text.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlTextArea areaControlHtml = _ParentControl.FindControl(strControlId) as HtmlTextArea;
                if (null != areaControlHtml)
                {
                    strValue = areaControlHtml.Value.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlInputPassword txtPassword = _ParentControl.FindControl(strControlId) as HtmlInputPassword;
                if (null != txtPassword)
                {
                    if (txtPassword.Disabled)
                        continue;
                    strValue = txtPassword.Value.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

                strControlId = "sel_" + strFieldName;
                DropDownList ddlControl = _ParentControl.FindControl(strControlId) as DropDownList;
                if (null != ddlControl)
                {
                    if (!ddlControl.Enabled)
                        continue;
                    strValue = ddlControl.SelectedValue.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlSelect ddlControlHtml = _ParentControl.FindControl(strControlId) as HtmlSelect;
                if (null != ddlControlHtml)
                {
                    if (ddlControlHtml.Disabled)
                        continue;
                    ListItem LiSelected = ddlControlHtml.Items[ddlControlHtml.SelectedIndex];
                    if (null == LiSelected)
                        continue;
                    strValue = LiSelected.Value.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

                strControlId = "cb_" + strFieldName;
                HtmlInputCheckBox cbControlHtml = _ParentControl.FindControl(strControlId) as HtmlInputCheckBox;
                if (null != cbControlHtml)
                {
                    if (cbControlHtml.Disabled)
                        continue;
                    strValue = (cbControlHtml.Checked ? Constants.Yes : Constants.No) + "";
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                CheckBox cbControl = _ParentControl.FindControl(strControlId) as CheckBox;
                if (null != cbControl)
                {
                    if (!cbControl.Enabled)
                        continue;
                    strValue = (cbControl.Checked ? Constants.Yes : Constants.No) + "";
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

            }
        }
        public static void PageFillEntity<TEntity>(System.Web.UI.Page _Page, TEntity _oE) where TEntity : IEntity,new()
        {
            if (null == _Page || null == _oE)
                return;
            //string[] astrFields = ReflectionUtil.GetObjectFieldsList(_oE, true, true);
            IEntityField[] astrFields = EntityAccess<TEntity>.Meta.EntityFields;
            if (null == astrFields || astrFields.Length == 0)
                return;
            string strControlId = "", strValue = "";
            foreach (IEntityField iEF in astrFields)
            {
                string strFieldName = iEF.FieldName;
                strControlId = "txt_" + strFieldName;
                HtmlInputText txtControlHtml = _Page.FindControl(strControlId) as HtmlInputText;
                if (null != txtControlHtml)
                {
                    if (txtControlHtml.Disabled)
                        continue;
                    strValue = txtControlHtml.Value.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                TextBox txtControl = _Page.FindControl(strControlId) as TextBox;
                if (null != txtControl)
                {
                    if (!txtControl.Enabled)
                        continue;
                    strValue = txtControl.Text.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlTextArea areaControlHtml = _Page.FindControl(strControlId) as HtmlTextArea;
                if (null != areaControlHtml)
                {
                    strValue = areaControlHtml.Value.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlInputPassword txtPassword = _Page.FindControl(strControlId) as HtmlInputPassword;
                if (null != txtPassword)
                {
                    if (txtPassword.Disabled)
                        continue;
                    strValue = txtPassword.Value.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

                strControlId = "sel_" + strFieldName;
                DropDownList ddlControl = _Page.FindControl(strControlId) as DropDownList;
                if (null != ddlControl)
                {
                    if (!ddlControl.Enabled)
                        continue;
                    strValue = ddlControl.SelectedValue.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                HtmlSelect ddlControlHtml = _Page.FindControl(strControlId) as HtmlSelect;
                if (null != ddlControlHtml)
                {
                    if (ddlControlHtml.Disabled)
                        continue;
                    ListItem LiSelected = ddlControlHtml.Items[ddlControlHtml.SelectedIndex];
                    if (null == LiSelected)
                        continue;
                    strValue = LiSelected.Value.Trim();
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

                strControlId = "cb_" + strFieldName;
                HtmlInputCheckBox cbControlHtml = _Page.FindControl(strControlId) as HtmlInputCheckBox;
                if (null != cbControlHtml)
                {
                    if (cbControlHtml.Disabled)
                        continue;
                    strValue = (cbControlHtml.Checked ? Constants.Yes : Constants.No) + "";
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }
                CheckBox cbControl = _Page.FindControl(strControlId) as CheckBox;
                if (null != cbControl)
                {
                    if (!cbControl.Enabled)
                        continue;
                    strValue = (cbControl.Checked ? Constants.Yes : Constants.No) + "";
                    iEF.SetValue<TEntity>(_oE, strValue);
                    //ReflectionUtil.SetObjectFieldValue(_oE, strFieldName, strValue);
                    continue;
                }

            }
        }
    }
}
