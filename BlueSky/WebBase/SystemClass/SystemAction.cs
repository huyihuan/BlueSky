using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using WebBase.Utilities;
namespace WebBase.SystemClass
{
	public class SystemAction : IEntity, IComparer<SystemAction>
	{
		public const string CONST_ACTIONTYPE_NORMAL = "normal";
		public const string CONST_ACTIONTYPE_JAVASCRIPT = "javascript";
		public const string CONST_ACTIONTYPE_HTTP = "http";
		public static Hashtable htActionTypeToName = new Hashtable(new Dictionary<string, string>
		{

			{
				"normal",
				"normal"
			},

			{
				"javascript",
				"javascript"
			},

			{
				"http",
				"http"
			}
		});
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
		public string Key
		{
			get;
			set;
		}
		public int IsDefault
		{
			get;
			set;
		}
		public int FunctionId
		{
			get;
			set;
		}
		public string ActionType
		{
			get;
			set;
		}
		public string ActionValue
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public string ControlName
		{
			get;
			set;
		}
		public string Tip
		{
			get;
			set;
		}
		public string IconName
		{
			get;
			set;
		}
		public string Target
		{
			get;
			set;
		}
		public int IsPopup
		{
			get;
			set;
		}
		public int Width
		{
			get;
			set;
		}
		public int Height
		{
			get;
			set;
		}
		public int IsResize
		{
			get;
			set;
		}
		public int IsMove
		{
			get;
			set;
		}
		public int IsIncludeMinBox
		{
			get;
			set;
		}
		public int IsIncludeMaxBox
		{
			get;
			set;
		}
		public int IsShowInTaskBar
		{
			get;
			set;
		}
		public int OrderId
		{
			get;
			set;
		}
		public int EntityCount
		{
			get;
			set;
		}
		public static void BindActionTypeList(ListControl _ltControl, bool _bIncludeSelect, bool _bIncludeAll)
		{
			if (null != _ltControl)
			{
				_ltControl.Items.Clear();
				if (_bIncludeSelect)
				{
					_ltControl.Items.Add(new ListItem("--请选择--", ""));
				}
				if (_bIncludeAll)
				{
					_ltControl.Items.Add(new ListItem("--全部--", ""));
				}
				foreach (string strKey in SystemAction.htActionTypeToName.Keys)
				{
					ListItem li = new ListItem();
					li.Text = string.Concat(SystemAction.htActionTypeToName[strKey]);
					li.Value = strKey;
					_ltControl.Items.Add(li);
				}
			}
		}
		public int Compare(SystemAction x, SystemAction y)
		{
			return x.OrderId - y.OrderId;
		}
		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj) || this.Key.Equals(((SystemAction)obj).Key);
		}
		public string ToActionJson()
		{
			StringBuilder sbJson = new StringBuilder();
			sbJson.Append("{");
			sbJson.Append(string.Format("actionType:'{0}',", this.ActionType));
			sbJson.Append(string.Format("actionKey:'{0}',", this.Key));
			sbJson.Append(string.Format("entityCount:{0},", this.EntityCount));
			sbJson.Append(string.Format("fn:{0},", this.FunctionId));
			sbJson.Append(string.Format("popup:{0},", this.IsPopup.ToString().ToLower()));
			sbJson.Append(string.Format("width:{0},", this.Width));
			sbJson.Append(string.Format("height:{0},", this.Height));
			sbJson.Append(string.Format("title:'{0}',", this.Name));
			sbJson.Append(string.Format("resize:{0},", (this.IsResize == 1).ToString().ToLower()));
			sbJson.Append(string.Format("maxbox:{0},", (this.IsIncludeMaxBox == 1).ToString().ToLower()));
			sbJson.Append(string.Format("minbox:{0},", (this.IsIncludeMinBox == 1).ToString().ToLower()));
			sbJson.Append(string.Format("move:{0},", (this.IsMove == 1).ToString().ToLower()));
			sbJson.Append(string.Format("actionValue:'{0}',", this.ActionValue));
			sbJson.Append(string.Format("iconURL:'{0}'", SystemUtil.ResovleActionImagePath(this.IconName)));
			sbJson.Append("}");
			return sbJson.ToString();
		}
		public static SystemAction Get(int _nId)
		{
			SystemAction result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemAction>.Access.Get(_nId);
			}
			return result;
		}
		public static SystemAction Get(string _strKey)
		{
			SystemAction result;
			if (string.IsNullOrEmpty(_strKey))
			{
				result = null;
			}
			else
			{
				SystemAction[] alist = EntityAccess<SystemAction>.Access.List(string.Format("[Key]='{0}'", _strKey));
				if (alist == null || alist.Length == 0)
				{
					result = null;
				}
				else
				{
					if (alist.Length > 1)
					{
						throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", EntityAccess<SystemAction>.Meta.EntityName, "Key", _strKey));
					}
					result = alist[0];
				}
			}
			return result;
		}
		public static SystemAction[] Get(int _nFuntionId, int _nRoleId)
		{
			SystemRoleActionPermission[] alist = SystemRoleActionPermission.GetRoleActions(_nRoleId, _nFuntionId);
			SystemAction[] result;
			if (alist == null || alist.Length == 0)
			{
				result = null;
			}
			else
			{
				List<string> ltActionId = new List<string>();
				SystemRoleActionPermission[] array = alist;
				for (int i = 0; i < array.Length; i++)
				{
					SystemRoleActionPermission oPermission = array[i];
					ltActionId.Add(string.Concat(oPermission.ActionId));
				}
				string strFilter = string.Format("Id in ({0})", string.Join(",", ltActionId.ToArray()));
				result = SystemAction.List(strFilter);
			}
			return result;
		}
		public static SystemAction[] GetFunctionAction(int _FunctionId)
		{
			SystemAction[] result;
			if (_FunctionId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemAction>.Access.List("FunctionId=" + _FunctionId);
			}
			return result;
		}
		public static SystemAction[] GetUserAction(int _nUserId, int _nFunctionId)
		{
			SystemAction[] result;
			if (_nUserId <= 0 || _nFunctionId <= 0)
			{
				result = null;
			}
			else
			{
				SystemUserRole[] alUserRoles = SystemUserRole.GetUserRoles(_nUserId);
				if (alUserRoles == null || alUserRoles.Length <= 0)
				{
					result = null;
				}
				else
				{
					List<SystemAction> ltAction = new List<SystemAction>();
					SystemUserRole[] array = alUserRoles;
					for (int i = 0; i < array.Length; i++)
					{
						SystemUserRole oRole = array[i];
						SystemAction[] alist = SystemAction.Get(_nFunctionId, oRole.RoleId);
						if (alist != null && alist.Length != 0)
						{
							SystemAction[] array2 = alist;
							for (int j = 0; j < array2.Length; j++)
							{
								SystemAction action = array2[j];
								if (ltAction.IndexOf(action) < 0)
								{
									ltAction.Add(action);
								}
							}
						}
					}
					SystemUserActionPermission[] alUserAction = SystemUserActionPermission.Get(_nUserId, _nFunctionId);
					if (alUserAction != null && alUserAction.Length > 0)
					{
						List<string> ltUserActionId = new List<string>();
						SystemUserActionPermission[] array3 = alUserAction;
						for (int i = 0; i < array3.Length; i++)
						{
							SystemUserActionPermission oPermission = array3[i];
							ltUserActionId.Add(string.Concat(oPermission.ActionId));
						}
						string strFilter = string.Format("Id in ({0})", string.Join(",", ltUserActionId.ToArray()));
						SystemAction[] alUActions = SystemAction.List(strFilter);
						SystemAction[] array2 = alUActions;
						for (int i = 0; i < array2.Length; i++)
						{
							SystemAction action = array2[i];
							if (!ltAction.Contains(action))
							{
								ltAction.Add(action);
							}
						}
					}
					ltAction.Sort(new SystemAction());
					result = ltAction.ToArray();
				}
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemAction oDel = SystemAction.Get(_nId);
			if (null != oDel)
			{
				SystemRoleActionPermission[] alRoleActions = SystemRoleActionPermission.GetByActionId(_nId);
				if (alRoleActions != null && alRoleActions.Length > 0)
				{
					SystemRoleActionPermission[] array = alRoleActions;
					for (int i = 0; i < array.Length; i++)
					{
						SystemRoleActionPermission aPermission = array[i];
						SystemRoleActionPermission.Delete(aPermission.Id);
					}
				}
				SystemUserActionPermission[] alUserActions = SystemUserActionPermission.GetByActionId(_nId);
				if (alUserActions != null && alUserActions.Length > 0)
				{
					SystemUserActionPermission[] array2 = alUserActions;
					for (int i = 0; i < array2.Length; i++)
					{
						SystemUserActionPermission aPermission2 = array2[i];
						SystemUserActionPermission.Delete(aPermission2.Id);
					}
				}
				EntityAccess<SystemAction>.Access.Delete(oDel);
			}
		}
		public static SystemAction[] List()
		{
			return EntityAccess<SystemAction>.Access.List();
		}
		public static SystemAction[] List(string _strFilter)
		{
			return EntityAccess<SystemAction>.Access.List(_strFilter);
		}
		public static SystemAction[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemAction>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
		public static int Save(SystemAction _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemAction>.Access.Save(_Entity);
			}
			return result;
		}
	}
}
