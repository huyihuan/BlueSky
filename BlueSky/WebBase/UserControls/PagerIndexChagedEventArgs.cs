using System;
namespace WebBase.UserControls
{
	public class PagerIndexChagedEventArgs : EventArgs
	{
		private int _PageIndex;
		private int _PageSize;
		public int PageIndex
		{
			get
			{
				return this._PageIndex;
			}
		}
		public int PageSize
		{
			get
			{
				return this._PageSize;
			}
		}
		public PagerIndexChagedEventArgs(int _nIndex, int _nSize)
		{
			this._PageIndex = _nIndex;
			this._PageSize = _nSize;
		}
	}
}
