using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
using System.Web.UI.WebControls;
namespace WebBase.SystemClass
{
	public class SystemOrganizationType : IEntity
	{
		[EntityField(FieldName = "Id", IsPrimaryKey = true)]
		public int Id
		{
			get;
			set;
		}
		public int ParentId
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public string Remark
		{
			get;
			set;
		}
		public static SystemOrganizationType Get(int _nId)
		{
			SystemOrganizationType result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemOrganizationType>.Access.Get(_nId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemOrganizationType oDel = SystemOrganizationType.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemOrganizationType>.Access.Delete(oDel);
			}
		}
		public static bool Exist(string _strName)
		{
			bool result;
			if (string.IsNullOrEmpty(_strName))
			{
				result = true;
			}
			else
			{
				string strFilter = string.Format("Name='{0}'", _strName);
				SystemOrganizationType[] alist = EntityAccess<SystemOrganizationType>.Access.List(strFilter);
				if (null == alist)
				{
					result = false;
				}
				else
				{
					if (alist.Length > 1)
					{
						throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", EntityAccess<SystemOrganizationType>.Meta.EntityName, "Key", _strName));
					}
					result = (alist.Length == 1);
				}
			}
			return result;
		}
		public static SystemOrganizationType[] List()
		{
			return EntityAccess<SystemOrganizationType>.Access.List();
		}
		public static SystemOrganizationType[] List(string _strFilter)
		{
			return EntityAccess<SystemOrganizationType>.Access.List(_strFilter);
		}
		public static SystemOrganizationType[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemOrganizationType>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
		private static void _BindList(ListControl _ltControl, bool _bClearItems, bool _bIncludeSel, bool _bIncludeAll, int _nParentId, bool _bSelectAll, int _nLevel, ListItem[] _alPrependItems)
		{
			if (null != _ltControl)
			{
				if (_bClearItems)
				{
					_ltControl.Items.Clear();
				}
				if (_bIncludeAll)
				{
					_ltControl.Items.Add(new ListItem("--全部--", ""));
				}
				if (_bIncludeSel)
				{
					_ltControl.Items.Add(new ListItem("--请选择--", ""));
				}
				if (_nLevel == 0 && _alPrependItems != null && _alPrependItems.Length >= 1)
				{
					_ltControl.Items.AddRange(_alPrependItems);
				}
				SystemOrganizationType[] alist = SystemOrganizationType.List("ParentId=" + _nParentId);
				if (alist != null && alist.Length != 0)
				{
					SystemOrganizationType[] array = alist;
					for (int i = 0; i < array.Length; i++)
					{
						SystemOrganizationType item = array[i];
						ListItem li = new ListItem("".PadLeft(_nLevel * 3, '.') + item.Name, string.Concat(item.Id));
						_ltControl.Items.Add(li);
						if (_bSelectAll)
						{
							SystemOrganizationType._BindList(_ltControl, false, false, false, item.Id, true, _nLevel + 1, null);
						}
					}
				}
			}
		}
		public static void BindList(ListControl _ltControl, bool _bClearItems, bool _bIncludeSel, bool _bIncludeAll, int _nParentId, bool _bSelectAll)
		{
			SystemOrganizationType._BindList(_ltControl, _bClearItems, _bIncludeSel, _bIncludeAll, _nParentId, _bSelectAll, 0, null);
		}
		public static void BindList(ListControl _ltControl, bool _bClearItems, bool _bIncludeSel, bool _bIncludeAll, int _nParentId, bool _bSelectAll, ListItem[] _alPrependItems)
		{
			SystemOrganizationType._BindList(_ltControl, _bClearItems, _bIncludeSel, _bIncludeAll, _nParentId, _bSelectAll, 0, _alPrependItems);
		}
		public static int Save(SystemOrganizationType _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemOrganizationType>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
