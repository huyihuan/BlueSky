using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using DataBase;
using WebSystemBase.Utilities;

namespace WebSystemBase.UserControls
{
    [DefaultProperty("")]
    [ToolboxData("<{0}:PagerNavication runat=\"Server\"></{0}:PagerNavication>"), Description("List View Pager")]
    public class PagerNavication : WebControl, INamingContainer
    {
        private DropDownList _ddlSizeList;
        private LinkButton _lbtnFirst;
        private LinkButton _lbtnPrev;
        private TextBox _txtPageIndex;
        private LinkButton _lbtnNext;
        private LinkButton _lbtnLast;
        private LinkButton _lbRefresh;

        //默认每页记录条数
        private int _DefaultSize = 10;
        //默认每页记录数列表
        private string _DefaultSizeList = "10,15,30";

        #region Properties

        public int PageIndex
        {
            get 
            {
                if (null == this.ViewState["PageIndex"])
                    return 1;
                return (int)this.ViewState["PageIndex"];
            }
            set
            {
                this.ViewState["PageIndex"] = value;
            }
        }

        public int PageSize 
        {
            get 
            {
                if (null == this.ViewState["PageSize"])
                    return _DefaultSize;
                return (int)this.ViewState["PageSize"];
            }
            set
            {
                this.ViewState["PageSize"] = value;
            }
        }

        public int RecordsCount
        {
            get
            {
                if (null == this.ViewState["RecordsCount"])
                    return 0;
                return (int)this.ViewState["RecordsCount"];
            }
            set
            {
                this.ViewState["RecordsCount"] = value;
            }
        }

        public int PageCount
        {
            get
            {
                return DataBase.Util.ParseInt(Math.Ceiling((double)this.RecordsCount / (this.PageSize == 0 ? _DefaultSize : this.PageSize)) + "", 0);
            }
        }

        public string PageSizeList
        {
            get
            {
                if (null == this.ViewState["PageSizeList"])
                    return _DefaultSizeList;
                return this.ViewState["PageSizeList"] + "";
            }
            set
            {
                this.ViewState["PageSizeList"] = value;
            }
        }

        public string PagerImageUrl
        {
            get
            {
                if (null == this.ViewState["PagerImageUrl"])
                    return "";
                return this.ViewState["PagerImageUrl"] + "";
            }
            set
            {
                this.ViewState["PagerImageUrl"] = value;
            }
        }

        public bool Refreshable
        {
            get
            {
                if (null == this.ViewState["Refreshable"])
                    return false;
                return (bool)this.ViewState["Refreshable"];
            }
            set
            {
                this.ViewState["Refreshable"] = value;
            }
        }

        #endregion

        #region ControlCreatedMethods

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.InitChildControls();
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Table;
            }
        }

        private void InitChildControls()
        {
            this.Controls.Clear();
            bool bIsFirstIndex = this.PageIndex == 1 || this.PageCount == 0;
            bool bIsLastIndex = this.PageCount == 0 || (this.PageCount != 0 && this.PageIndex == this.PageCount);

            //每页数DropDownList
            this._ddlSizeList = new DropDownList();
            this._ddlSizeList.ID = "SizeList";
            this._ddlSizeList.ToolTip = "选择每页显示的记录条数！";
            string strPageSizeList = this.PageSizeList;
            if ("" == strPageSizeList)
                strPageSizeList = _DefaultSizeList;
            List<string> ltSizes = new List<string>(strPageSizeList.Split(new char[] { ',', '#', '$', '_', '&', '*' }));
            foreach (string strSize in ltSizes)
            {
                this._ddlSizeList.Items.Add(strSize);
            }
            this._ddlSizeList.SelectedValue = (ltSizes.Contains(this.PageSize + "") ? this.PageSize : _DefaultSize) + "";
            this._ddlSizeList.AutoPostBack = true;
            this._ddlSizeList.SelectedIndexChanged +=new EventHandler(_ddlSizeList_SelectedIndexChanged);
            this.Controls.Add(_ddlSizeList);

            //首页
            this._lbtnFirst = new LinkButton();
            this._lbtnFirst.ID = "FirstIndex";
            this._lbtnFirst.ToolTip = "首页";
            if (bIsFirstIndex)
            {
                this._lbtnFirst.Enabled = false;
                this._lbtnFirst.CssClass = "pager_first_disabled";
            }
            else
            {
                this._lbtnFirst.CssClass = "pager_first";
                this._lbtnFirst.Click += new EventHandler(_lbtnFirst_Click);
            }
            this.Controls.Add(_lbtnFirst);

            //向前一页
            this._lbtnPrev = new LinkButton();
            this._lbtnPrev.ID = "PrevIndex";
            this._lbtnPrev.ToolTip = "向前一页";
            if (bIsFirstIndex)
            {
                this._lbtnPrev.Enabled = false;
                this._lbtnPrev.CssClass = "pager_prev_disabled";
            }
            else
            {
                this._lbtnPrev.CssClass = "pager_prev";
                this._lbtnPrev.Click += new EventHandler(_lbtnPrev_Click);
            }
            this.Controls.Add(_lbtnPrev);

            //当前页TextBox
            this._txtPageIndex = new TextBox();
            this._txtPageIndex.ID = "Index";
            this._txtPageIndex.ToolTip = string.Format("当前第{0}页，输入页码后敲击“回车”键跳转！", this.PageIndex);
            this._txtPageIndex.Width = 25;
            this._txtPageIndex.Text = this.PageIndex + "";
            this._txtPageIndex.CssClass = "txt-normal";
            this._txtPageIndex.TextChanged += new EventHandler(_txtPageIndex_TextChanged);
            this._txtPageIndex.AutoPostBack = true;
            this.Controls.Add(_txtPageIndex);

            //向后一页
            this._lbtnNext = new LinkButton();
            this._lbtnNext.ID = "NextIndex";
            this._lbtnNext.ToolTip = "向后一页";
            if (bIsLastIndex)
            {
                this._lbtnNext.Enabled = false;
                this._lbtnNext.CssClass = "pager_next_disabled";
            }
            else
            {
                this._lbtnNext.CssClass = "pager_next";
                this._lbtnNext.Click += new EventHandler(_lbtnNext_Click);
            }
            this.Controls.Add(_lbtnNext);

            //尾页
            this._lbtnLast = new LinkButton();
            this._lbtnLast.ID = "LastIndex";
            this._lbtnLast.ToolTip = "尾页";
            if (bIsLastIndex)
            {
                this._lbtnLast.Enabled = false;
                this._lbtnLast.CssClass = "pager_last_disabled";
            }
            else
            {
                this._lbtnLast.CssClass = "pager_last";
                this._lbtnLast.Click += new EventHandler(_lbtnLast_Click);
            }
            this.Controls.Add(_lbtnLast);

            //刷新按钮
            if (this.Refreshable)
            {
                this._lbRefresh = new LinkButton();
                this._lbRefresh.ID = "Refresh";
                this._lbRefresh.ToolTip = "刷新列表";
                this._lbRefresh.CssClass = "pager_refresh";
                this._lbRefresh.Click +=new EventHandler(_lbRefresh_Click);
                this.Controls.Add(_lbRefresh);
            }

            this.ChildControlsCreated = true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.CreateChildControls();
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "pager");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            base.Render(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write("&nbsp;");
            _ddlSizeList.RenderControl(writer);
            writer.Write("&nbsp;|&nbsp;&nbsp;");
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _lbtnFirst.RenderControl(writer);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _lbtnPrev.RenderControl(writer);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write("&nbsp;");
            _txtPageIndex.RenderControl(writer);
            writer.Write(string.Format("<label style=\"margin:0px 2px;vertical-align:middle;\">/</label>{0}&nbsp;", this.PageCount));
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _lbtnNext.RenderControl(writer);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _lbtnLast.RenderControl(writer);
            writer.RenderEndTag();

            if (this.Refreshable)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("&nbsp;|&nbsp;");
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                _lbRefresh.RenderControl(writer);
                writer.RenderEndTag();
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(string.Format("每页&nbsp;{0}&nbsp;条,共&nbsp;{1}&nbsp;条&nbsp;", this.PageSize,this.RecordsCount));
            writer.RenderEndTag();

            writer.RenderEndTag();
            //base.RenderContents(writer);
        }

        #endregion

        #region PagerEventMethods

        public delegate void PagerIndexChagedHandler(object sender, PagerIndexChagedEventArgs e);

        private static object PagerIndexChagedEventKey = new object();
        public event PagerIndexChagedHandler PagerIndexChaged
        {
            add
            {
                Events.AddHandler(PagerIndexChagedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(PagerIndexChagedEventKey, value);
            }
        }

        public virtual void OnPagerIndexChaged(PagerIndexChagedEventArgs e)
        {
            PagerIndexChagedHandler ChangeHandler = Events[PagerIndexChagedEventKey] as PagerIndexChagedHandler;
            if (null != ChangeHandler)
                ChangeHandler(this, e);
        }

        #endregion

        #region PagerChangeMethods

        private void DoEvent()
        {
            PagerIndexChagedEventArgs pe = new PagerIndexChagedEventArgs(this.PageIndex, this.PageSize);
            OnPagerIndexChaged(pe);
        }

        protected void _ddlSizeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageSize = int.Parse(_ddlSizeList.SelectedValue);
            this.PageIndex = 1;
            DoEvent();
        }

        protected void _lbtnFirst_Click(object sender, EventArgs e)
        {
            this.PageIndex = 1;
            DoEvent();
        }

        protected void _lbtnPrev_Click(object sender, EventArgs e)
        {
            this.PageIndex--;
            DoEvent();
        }

        protected void _txtPageIndex_TextChanged(object sender, EventArgs e)
        {
            int nIndex = Util.ParseInt(_txtPageIndex.Text.Trim(), -1);
            if (nIndex == -1 || nIndex == this.PageIndex)
                return;
            if (nIndex <= 0 || nIndex > this.PageCount)
            {
                PageUtil.PageAlert(this.Page, string.Format("请输入 1～{0} 之间的正整数！", this.PageCount));
                return;
            }
            this.PageIndex = nIndex;
            DoEvent();
        }

        protected void _lbtnNext_Click(object sender, EventArgs e)
        {
            this.PageIndex++;
            DoEvent();
        }

        protected void _lbtnLast_Click(object sender, EventArgs e)
        {
            this.PageIndex = this.PageCount;
            DoEvent();
        }

        protected void _lbRefresh_Click(object sender, EventArgs e)
        {
            DoEvent();
        }

        #endregion
    }

    public class PagerIndexChagedEventArgs : EventArgs
    {
        private int _PageIndex;
        private int _PageSize;
        public int PageIndex
        {
            get { return this._PageIndex; }
        }
        public int PageSize
        {
            get { return this._PageSize; }
        }

        public PagerIndexChagedEventArgs(int _nIndex, int _nSize)
        {
            this._PageIndex = _nIndex;
            this._PageSize = _nSize;
        }
    }
}
