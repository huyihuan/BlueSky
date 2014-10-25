using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using BlueSky.Utilities;
using System;
namespace WebBase.SystemClass
{
	public class UserInformation : IEntity
	{
		public const string CONST_STR_DEFAULTPASSWORD = "123456";
		[EntityField(FieldName = "Id", IsPrimaryKey = true)]
		public int Id
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
		public string Password
		{
			get;
			set;
		}
		public string NickName
		{
			get;
			set;
		}
		public int Gender
		{
			get;
			set;
		}
		public string CardID
		{
			get;
			set;
		}
		public int Age
		{
			get;
			set;
		}
		public string Email
		{
			get;
			set;
		}
		public string PostCode
		{
			get;
			set;
		}
		public string MSN
		{
			get;
			set;
		}
		public string Country
		{
			get;
			set;
		}
		public string QQ
		{
			get;
			set;
		}
        public string Tel
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string Remark
        {
            get;
            set;
        }
		public string PropertyRoleName
		{
			get
			{
				SystemUserRole[] alist = SystemUserRole.GetUserRoles(this.Id);
				string result;
				if (null == alist)
				{
					result = "";
				}
				else
				{
					string strRoleName = "";
					SystemUserRole[] array = alist;
					for (int i = 0; i < array.Length; i++)
					{
						SystemUserRole item = array[i];
						if ("" != strRoleName)
						{
							strRoleName += ",";
						}
						strRoleName += item.PropertyRoleName;
					}
					result = strRoleName;
				}
				return result;
			}
		}
		public string PropertyGender
		{
			get
			{
				return (this.Gender == 1) ? "男" : "女";
			}
		}
		public static UserInformation Get(int _nId)
		{
			UserInformation result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<UserInformation>.Access.Get(_nId);
			}
			return result;
		}
		public static UserInformation Get(string _strUserName)
		{
			UserInformation oGet = new UserInformation();
			oGet.UserName = _strUserName;
			UserInformation[] alist = EntityAccess<UserInformation>.Access.List(oGet);
			UserInformation result;
			if (alist == null || alist.Length == 0)
			{
				result = null;
			}
			else
			{
				if (alist.Length > 1)
				{
					throw new Exception(string.Format("{0}(UserName:{1}) exist mutil records！", EntityAccess<UserInformation>.Meta.EntityName, _strUserName));
				}
				result = alist[0];
			}
			return result;
		}
		public static UserInformation[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<UserInformation>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
		public static void Delete(int _nId)
		{
			UserInformation oDel = UserInformation.Get(_nId);
			if (null != oDel)
			{
				SystemUserModulePermission[] alModulePermission = SystemUserModulePermission.GetUserModulePermission(_nId);
				if (alModulePermission != null && alModulePermission.Length != 0)
				{
					SystemUserModulePermission[] array = alModulePermission;
					for (int i = 0; i < array.Length; i++)
					{
						SystemUserModulePermission oModulePermission = array[i];
						SystemUserFunctionPermission[] alFuncitnPermission = SystemUserFunctionPermission.GetUserFunctionPermission(_nId, oModulePermission.ModuleId);
						if (alFuncitnPermission != null && alFuncitnPermission.Length != 0)
						{
							SystemUserFunctionPermission[] array2 = alFuncitnPermission;
							for (int j = 0; j < array2.Length; j++)
							{
								SystemUserFunctionPermission oFunctionPermission = array2[j];
								SystemUserActionPermission[] alActionPermission = SystemUserActionPermission.Get(_nId, oFunctionPermission.FunctionId);
								if (alFuncitnPermission != null && alFuncitnPermission.Length != 0)
								{
									SystemUserActionPermission[] array3 = alActionPermission;
									for (int k = 0; k < array3.Length; k++)
									{
										SystemUserActionPermission oActionPermission = array3[k];
										SystemUserActionPermission.Delete(oActionPermission.Id);
									}
									SystemUserFunctionPermission.Delete(oFunctionPermission.Id);
								}
							}
							SystemUserModulePermission.Delete(oModulePermission.Id);
						}
					}
				}
				SystemUserRole[] alRole = SystemUserRole.GetUserRoles(_nId);
				if (alRole != null && alRole.Length != 0)
				{
					SystemUserRole[] array4 = alRole;
					for (int i = 0; i < array4.Length; i++)
					{
						SystemUserRole oRole = array4[i];
						SystemUserRole.Delete(oRole.Id);
					}
				}
				EntityAccess<UserInformation>.Access.Delete(oDel);
			}
		}
		public static int Save(UserInformation _Entity)
		{
			int result;
			if (null == _Entity)
			{
				result = -1;
			}
			else
			{
				result = EntityAccess<UserInformation>.Access.Save(_Entity);
			}
			return result;
		}
		public static bool ExistUser(string _strUserName)
		{
			bool result;
			if (string.IsNullOrEmpty(_strUserName))
			{
				result = false;
			}
			else
			{
				UserInformation oExist = new UserInformation();
				oExist.UserName = _strUserName;
				int nCount = EntityAccess<UserInformation>.Access.Count(oExist);
				if (nCount > 1)
				{
					throw new Exception(string.Format("{0}(UserName:{1}) exist mutil records！", EntityAccess<UserInformation>.Meta.EntityName, _strUserName));
				}
				result = (nCount == 1);
			}
			return result;
		}
		public static bool ExistUser(string _strUserName, string _strPassword)
		{
			bool result;
			if (string.IsNullOrEmpty(_strUserName) || string.IsNullOrEmpty(_strPassword))
			{
				result = false;
			}
			else
			{
				UserInformation oExist = new UserInformation();
				oExist.UserName = _strUserName;
				oExist.Password = CryptUtil.MD5Encrypt(_strPassword);
				int nCount = EntityAccess<UserInformation>.Access.Count(oExist);
				if (nCount > 1)
				{
					throw new Exception(string.Format("{0}(UserName:{1}) exist mutil records！", EntityAccess<UserInformation>.Meta.EntityName, _strUserName));
				}
				result = (nCount == 1);
			}
			return result;
		}
	}
}
