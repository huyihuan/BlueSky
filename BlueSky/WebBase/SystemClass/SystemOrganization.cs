using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
using System.Web.UI.WebControls;
namespace WebBase.SystemClass
{
	public class SystemOrganization : IEntity
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
		public int TypeId
		{
			get;
			set;
		}
		public string Remark
		{
			get;
			set;
		}
		public string OrganizationTypeName
		{
			get
			{
				SystemOrganizationType oType = SystemOrganizationType.Get(this.TypeId);
				string result;
				if (null == oType)
				{
					result = "";
				}
				else
				{
					result = oType.Name;
				}
				return result;
			}
		}
		public static SystemOrganization Get(int _nId)
		{
			SystemOrganization result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemOrganization>.Access.Get(_nId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemOrganization oDel = SystemOrganization.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemOrganization>.Access.Delete(oDel);
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
				SystemOrganization oExist = new SystemOrganization();
				oExist.Name = _strName;
				SystemOrganization[] alist = EntityAccess<SystemOrganization>.Access.List(oExist);
				if (null == alist)
				{
					result = false;
				}
				else
				{
					if (alist.Length > 1)
					{
						throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", EntityAccess<SystemOrganization>.Meta.EntityName, "Key", _strName));
					}
					result = (alist.Length == 1);
				}
			}
			return result;
		}
		public static SystemOrganization[] List()
		{
			return EntityAccess<SystemOrganization>.Access.List();
		}
		public static SystemOrganization[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
			return EntityAccess<SystemOrganization>.Access.List(__strFilter, "", __nPageIndex, __nPageSize);
		}
		public static void BindList(ListControl _ltControl, bool _bIncludeSel, bool _bIncludeAll)
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
				SystemOrganization[] alist = SystemOrganization.List();
				if (alist != null && alist.Length != 0)
				{
					SystemOrganization[] array = alist;
					for (int i = 0; i < array.Length; i++)
					{
						SystemOrganization item = array[i];
						ListItem li = new ListItem(item.Name, string.Concat(item.Id));
						_ltControl.Items.Add(li);
					}
				}
			}
		}
		public static int Save(SystemOrganization _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemOrganization>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
