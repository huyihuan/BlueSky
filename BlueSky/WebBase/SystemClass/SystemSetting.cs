using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemSetting : IEntity
	{
		public const string KEY_ENABLEAUTOLOGIN = "KEY_ENABLEAUTOLOGIN";
		public const string KEY_AUTOLOGINUSERNAME = "KEY_AUTOLOGINUSERNAME";
		public const string KEY_AUTOLOGINPASSWORD = "KEY_AUTOLOGINPASSWORD";
		[EntityField(FieldName = "Id", IsPrimaryKey = true)]
		public int Id
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		public string Key
		{
			get;
			set;
		}
		public string Value
		{
			get;
			set;
		}
		public string Remark
		{
			get;
			set;
		}
		public static SystemSetting Get(int _nId)
		{
			SystemSetting result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemSetting>.Access.Get(_nId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemSetting oDel = SystemSetting.Get(_nId);
			if (null != oDel)
			{
				SystemSetting.Delete(oDel);
			}
		}
		public static void Delete(SystemSetting _oDel)
		{
			if (null != _oDel)
			{
				EntityAccess<SystemSetting>.Access.Delete(_oDel);
			}
		}
		public static void DeleteUserSetting(int _nUserId)
		{
			if (_nUserId > 0)
			{
				SystemSetting oGet = new SystemSetting();
				oGet.UserId = _nUserId;
				SystemSetting[] alist = EntityAccess<SystemSetting>.Access.List(oGet);
				if (alist != null && alist.Length >= 1)
				{
					SystemSetting[] array = alist;
					for (int i = 0; i < array.Length; i++)
					{
						SystemSetting oSetting = array[i];
						SystemSetting.Delete(oSetting);
					}
				}
			}
		}
		public static int Save(SystemSetting _oSave)
		{
			int result;
			if (null == _oSave)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<SystemSetting>.Access.Save(_oSave);
			}
			return result;
		}
		public static SystemSetting GetSetting(string _strKey)
		{
			return SystemSetting.GetSetting(_strKey, -1);
		}
		public static void SetSetting(string _strKey, object _oValue)
		{
			SystemSetting.SetSetting(_strKey, _oValue, -1);
		}
		public static SystemSetting GetSetting(string _strKey, int _nUserId)
		{
			SystemSetting result;
			if (string.IsNullOrEmpty(_strKey))
			{
				result = null;
			}
			else
			{
				string strFilter = string.Format("Key='{0}'", _strKey);
				if (_nUserId > 0)
				{
					strFilter = strFilter + " and UserId=" + _nUserId;
				}
				else
				{
					strFilter += " and (UserId is null or UserId=0)";
				}
				SystemSetting[] alist = EntityAccess<SystemSetting>.Access.List(strFilter);
				if (alist == null || alist.Length == 0)
				{
					result = null;
				}
				else
				{
					result = alist[0];
				}
			}
			return result;
		}
		public static void SetSetting(string _strKey, object _oValue, int _nUserId)
		{
			if (!string.IsNullOrEmpty(_strKey))
			{
				SystemSetting oSave = SystemSetting.GetSetting(_strKey, _nUserId);
				if (null == oSave)
				{
					oSave = new SystemSetting();
					oSave.Key = _strKey;
				}
				oSave.Value = ((_oValue == null) ? null : _oValue.ToString());
				if (_nUserId > 0)
				{
					oSave.UserId = _nUserId;
				}
				SystemSetting.Save(oSave);
			}
		}
	}
}
