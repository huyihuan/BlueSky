using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
namespace BlueSky.Utilities
{
	public class ReflectionUtil
	{
		public static string[] GetObjectFieldsList(object _oSource, bool _bIncludeProperty, bool _bOnlyCanWrite)
		{
			string[] result;
			if (null == _oSource)
			{
				result = null;
			}
			else
			{
				List<string> ltReturnList = new List<string>();
				Type oTp = _oSource.GetType();
				FieldInfo[] alFields = oTp.GetFields();
                for (int i = 0; i < alFields.Length; i++)
				{
                    FieldInfo item = alFields[i];
					if (!item.IsStatic)
					{
						ltReturnList.Add(item.Name);
					}
				}
				if (_bIncludeProperty)
				{
					PropertyInfo[] alProperties = oTp.GetProperties();
                    for (int i = 0; i < alProperties.Length; i++)
					{
                        if (_bOnlyCanWrite && !alProperties[i].CanWrite)
                        {
                            continue;
                        }
                        ltReturnList.Add(alProperties[i].Name);
					}
				}
				result = ltReturnList.ToArray();
			}
			return result;
		}
		public static Hashtable GetObjectFieldValueHash(object _oSource)
		{
			Hashtable result;
			if (null == _oSource)
			{
				result = null;
			}
			else
			{
				Hashtable htFieldValue = new Hashtable();
				Type oTp = _oSource.GetType();
				FieldInfo[] alFields = oTp.GetFields();
                for (int i = 0; i < alFields.Length; i++)
				{
                    FieldInfo item = alFields[i];
					if (!item.IsStatic)
					{
						htFieldValue[item.Name] = item.GetValue(_oSource);
					}
				}
				PropertyInfo[] alProperties = oTp.GetProperties();
                for (int i = 0; i < alProperties.Length; i++)
				{
                    PropertyInfo item2 = alProperties[i];
					htFieldValue[item2.Name] = item2.GetValue(_oSource, null);
				}
				result = htFieldValue;
			}
			return result;
		}
		public static object GetObjectFieldValue(object _oSource, string _strFieldName)
		{
			object result;
			if (_oSource == null || string.IsNullOrEmpty(_strFieldName))
			{
				result = null;
			}
			else
			{
				Type oTp = _oSource.GetType();
				FieldInfo field = oTp.GetField(_strFieldName);
				if (null == field)
				{
					result = null;
				}
				else
				{
					result = field.GetValue(_oSource);
				}
			}
			return result;
		}
		public static void SetObjectFieldValue(object _oSource, string _strFieldName, object _oValue)
		{
			if (_oSource != null && !string.IsNullOrEmpty(_strFieldName) && null != _oValue)
			{
				Type oTp = _oSource.GetType();
				FieldInfo field = oTp.GetField(_strFieldName);
				if (null != field)
				{
					object oValue = new object();
                    TypeCode oTc = Type.GetTypeCode(field.FieldType);
                    if (oTc == TypeCode.Int16 || oTc == TypeCode.Int32)
					{
						oValue = TypeUtil.ParseInt(string.Concat(_oValue), 0);
					}
					else if (oTc == TypeCode.String)
					{
						oValue = string.Concat(_oValue);
					}
                    else if (oTc == TypeCode.Double)
					{
						oValue = TypeUtil.ParseDouble(string.Concat(_oValue), 0.0);
					}
					else if(oTc == TypeCode.Int64)
					{
						oValue = TypeUtil.ParseLong(string.Concat(_oValue), 0L);
					}
                    else if (oTc == TypeCode.DateTime)
                    { 
                        DateTime dt = new DateTime();
                        if (!DateTime.TryParse(_oValue.ToString(), out dt))
                        {
                            return;    
                        }
                        oValue = dt;
                    }
                    else if (oTc == TypeCode.Boolean)
                    {
                        Boolean b = true;
                        if (!Boolean.TryParse(_oValue.ToString(),out b))
                        {
                            return;
                        }
                        oValue = b;
                    }
				    field.SetValue(_oSource, oValue);
				}
			}
		}
	}
}
