using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
using System.Web.UI.WebControls;
namespace WebBase.SystemClass
{
	public class SystemRole : IEntity
	{
		[EntityField(FieldName = "Id", IsPrimaryKey = true)]
		public int Id
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public static SystemRole Get(int _nId)
		{
			SystemRole result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemRole>.Access.Get(_nId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemRole oDel = SystemRole.Get(_nId);
			if (null != oDel)
			{
                SystemUserRole[] alRoleUsers = SystemUserRole.GetRoleUsers(_nId);
                if (null != alRoleUsers && alRoleUsers.Length >= 1)
                {
                    foreach (SystemUserRole oUserRole in alRoleUsers)
                        SystemUserRole.Delete(oUserRole.Id);
                }
				EntityAccess<SystemRole>.Access.Delete(oDel);
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
				int nCount = EntityAccess<SystemRole>.Access.Count(string.Format("Name='{0}'", _strName));
				if (nCount == 0)
				{
					result = false;
				}
				else
				{
					if (nCount > 1)
					{
						throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", EntityAccess<SystemRole>.Meta.EntityName, "Name", _strName));
					}
					result = (nCount == 1);
				}
			}
			return result;
		}
		public static SystemRole[] List()
		{
			return EntityAccess<SystemRole>.Access.List();
		}
		public static SystemRole[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemRole>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
		public static void BindList(ListControl _ltControl, bool _bIncludeSel, bool _bIncludeAll, bool _bIncludeTip)
		{
			if (null != _ltControl)
			{
				_ltControl.Items.Clear();
				if (_bIncludeAll)
				{
					_ltControl.Items.Add(new ListItem("--全部--", ""));
				}
				if (_bIncludeSel)
				{
					_ltControl.Items.Add(new ListItem("--请选择--", ""));
				}
				SystemRole[] alist = SystemRole.List();
				if (alist != null && alist.Length != 0)
				{
					SystemRole[] array = alist;
					for (int i = 0; i < array.Length; i++)
					{
						SystemRole item = array[i];
						ListItem li = new ListItem(item.Name, string.Concat(item.Id));
						if (_bIncludeTip)
						{
							li.Attributes["title"] = item.Description;
						}
						_ltControl.Items.Add(li);
					}
				}
			}
		}
		public static int Save(SystemRole _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemRole>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
