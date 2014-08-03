using BlueSky.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
namespace WebBase.UserControls
{
	[DefaultProperty(""), Description("List View Pager"), ToolboxData("<{0}:PagerNavication runat=\"Server\"></{0}:PagerNavication>")]
	public class PagerNavication : WebControl, INamingContainer
	{
		public delegate void PagerIndexChagedHandler(object sender, PagerIndexChagedEventArgs e);
		private DropDownList _ddlSizeList;
		private LinkButton _lbtnFirst;
		private LinkButton _lbtnPrev;
		private TextBox _txtPageIndex;
		private LinkButton _lbtnNext;
		private LinkButton _lbtnLast;
		private LinkButton _lbRefresh;
		private int _DefaultSize = 10;
		private string _DefaultSizeList = "10,15,30";
		private static object PagerIndexChagedEventKey = new object();
		public event PagerNavication.PagerIndexChagedHandler PagerIndexChaged
		{
			add
			{
				base.Events.AddHandler(PagerNavication.PagerIndexChagedEventKey, value);
			}
			remove
			{
				base.Events.RemoveHandler(PagerNavication.PagerIndexChagedEventKey, value);
			}
		}
		public int PageIndex
		{
			get
			{
				int result;
				if (null == this.ViewState["PageIndex"])
				{
					result = 1;
				}
				else
				{
					result = (int)this.ViewState["PageIndex"];
				}
				return result;
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
				int result;
				if (null == this.ViewState["PageSize"])
				{
					result = this._DefaultSize;
				}
				else
				{
					result = (int)this.ViewState["PageSize"];
				}
				return result;
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
				int result;
				if (null == this.ViewState["RecordsCount"])
				{
					result = 0;
				}
				else
				{
					result = (int)this.ViewState["RecordsCount"];
				}
				return result;
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
				return TypeUtil.ParseInt(string.Concat(Math.Ceiling((double)this.RecordsCount / (double)((this.PageSize == 0) ? this._DefaultSize : this.PageSize))), 0);
			}
		}
		public string PageSizeList
		{
			get
			{
				string result;
				if (null == this.ViewState["PageSizeList"])
				{
					result = this._DefaultSizeList;
				}
				else
				{
					result = string.Concat(this.ViewState["PageSizeList"]);
				}
				return result;
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
				string result;
				if (null == this.ViewState["PagerImageUrl"])
				{
					result = "";
				}
				else
				{
					result = string.Concat(this.ViewState["PagerImageUrl"]);
				}
				return result;
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
				return null != this.ViewState["Refreshable"] && (bool)this.ViewState["Refreshable"];
			}
			set
			{
				this.ViewState["Refreshable"] = value;
			}
		}
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}
		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			this.InitChildControls();
		}
		private void InitChildControls()
		{
			this.Controls.Clear();
			bool bIsFirstIndex = this.PageIndex == 1 || this.PageCount == 0;
			bool bIsLastIndex = this.PageCount == 0 || (this.PageCount != 0 && this.PageIndex == this.PageCount);
			this._ddlSizeList = new DropDownList();
			this._ddlSizeList.ID = "SizeList";
			this._ddlSizeList.ToolTip = "选择每页显示的记录条数！";
			string strPageSizeList = this.PageSizeList;
			if ("" == strPageSizeList)
			{
				strPageSizeList = this._DefaultSizeList;
			}
			List<string> ltSizes = new List<string>(strPageSizeList.Split(new char[]
			{
				',',
				'#',
				'$',
				'_',
				'&',
				'*'
			}));
			foreach (string strSize in ltSizes)
			{
				this._ddlSizeList.Items.Add(strSize);
			}
			this._ddlSizeList.SelectedValue = string.Concat(ltSizes.Contains(string.Concat(this.PageSize)) ? this.PageSize : this._DefaultSize);
			this._ddlSizeList.AutoPostBack = true;
			this._ddlSizeList.SelectedIndexChanged += new EventHandler(this._ddlSizeList_SelectedIndexChanged);
			this.Controls.Add(this._ddlSizeList);
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
				this._lbtnFirst.Click += new EventHandler(this._lbtnFirst_Click);
			}
			this.Controls.Add(this._lbtnFirst);
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
				this._lbtnPrev.Click += new EventHandler(this._lbtnPrev_Click);
			}
			this.Controls.Add(this._lbtnPrev);
			this._txtPageIndex = new TextBox();
			this._txtPageIndex.ID = "Index";
			this._txtPageIndex.ToolTip = string.Format("当前第{0}页，输入页码后敲击“回车”键跳转！", this.PageIndex);
			this._txtPageIndex.Width = 25;
			this._txtPageIndex.Text = string.Concat(this.PageIndex);
			this._txtPageIndex.CssClass = "txt-normal";
			this._txtPageIndex.TextChanged += new EventHandler(this._txtPageIndex_TextChanged);
			this._txtPageIndex.AutoPostBack = true;
			this.Controls.Add(this._txtPageIndex);
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
				this._lbtnNext.Click += new EventHandler(this._lbtnNext_Click);
			}
			this.Controls.Add(this._lbtnNext);
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
				this._lbtnLast.Click += new EventHandler(this._lbtnLast_Click);
			}
			this.Controls.Add(this._lbtnLast);
			if (this.Refreshable)
			{
				this._lbRefresh = new LinkButton();
				this._lbRefresh.ID = "Refresh";
				this._lbRefresh.ToolTip = "刷新列表";
				this._lbRefresh.CssClass = "pager_refresh";
				this._lbRefresh.Click += new EventHandler(this._lbRefresh_Click);
				this.Controls.Add(this._lbRefresh);
			}
			base.ChildControlsCreated = true;
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
			this._ddlSizeList.RenderControl(writer);
			writer.Write("&nbsp;|&nbsp;&nbsp;");
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			this._lbtnFirst.RenderControl(writer);
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			this._lbtnPrev.RenderControl(writer);
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;");
			this._txtPageIndex.RenderControl(writer);
			writer.Write(string.Format("<label style=\"margin:0px 2px;vertical-align:middle;\">/</label>{0}&nbsp;", this.PageCount));
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			this._lbtnNext.RenderControl(writer);
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			this._lbtnLast.RenderControl(writer);
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
				this._lbRefresh.RenderControl(writer);
				writer.RenderEndTag();
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write(string.Format("每页&nbsp;{0}&nbsp;条,共&nbsp;{1}&nbsp;条&nbsp;", this.PageSize, this.RecordsCount));
			writer.RenderEndTag();
			writer.RenderEndTag();
		}
		public virtual void OnPagerIndexChaged(PagerIndexChagedEventArgs e)
		{
			PagerNavication.PagerIndexChagedHandler ChangeHandler = base.Events[PagerNavication.PagerIndexChagedEventKey] as PagerNavication.PagerIndexChagedHandler;
			if (null != ChangeHandler)
			{
				ChangeHandler(this, e);
			}
		}
		private void DoEvent()
		{
			PagerIndexChagedEventArgs pe = new PagerIndexChagedEventArgs(this.PageIndex, this.PageSize);
			this.OnPagerIndexChaged(pe);
		}
		protected void _ddlSizeList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.PageSize = int.Parse(this._ddlSizeList.SelectedValue);
			this.PageIndex = 1;
			this.DoEvent();
		}
		protected void _lbtnFirst_Click(object sender, EventArgs e)
		{
			this.PageIndex = 1;
			this.DoEvent();
		}
		protected void _lbtnPrev_Click(object sender, EventArgs e)
		{
			this.PageIndex--;
			this.DoEvent();
		}
		protected void _txtPageIndex_TextChanged(object sender, EventArgs e)
		{
			int nIndex = TypeUtil.ParseInt(this._txtPageIndex.Text.Trim(), -1);
			if (nIndex != -1 && nIndex != this.PageIndex)
			{
				if (nIndex <= 0 || nIndex > this.PageCount)
				{
					PageUtil.PageAlert(this.Page, string.Format("请输入 1～{0} 之间的正整数！", this.PageCount));
				}
				else
				{
					this.PageIndex = nIndex;
					this.DoEvent();
				}
			}
		}
		protected void _lbtnNext_Click(object sender, EventArgs e)
		{
			this.PageIndex++;
			this.DoEvent();
		}
		protected void _lbtnLast_Click(object sender, EventArgs e)
		{
			this.PageIndex = this.PageCount;
			this.DoEvent();
		}
		protected void _lbRefresh_Click(object sender, EventArgs e)
		{
			this.DoEvent();
		}
	}
}
