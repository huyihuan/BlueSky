using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
namespace BlueSky.Utilities
{
	public class ReflectionUtil
	{
		public static string[] GetObjectFieldsList(object _oSource, bool _bIncludeProperty)
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
				FieldInfo[] array = alFields;
				for (int i = 0; i < array.Length; i++)
				{
					FieldInfo item = array[i];
					if (!item.IsStatic)
					{
						ltReturnList.Add(item.Name);
					}
				}
				if (_bIncludeProperty)
				{
					PropertyInfo[] alProperties = oTp.GetProperties();
					PropertyInfo[] array2 = alProperties;
					for (int i = 0; i < array2.Length; i++)
					{
						PropertyInfo item2 = array2[i];
						ltReturnList.Add(item2.Name);
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
				FieldInfo[] array = alFields;
				for (int i = 0; i < array.Length; i++)
				{
					FieldInfo item = array[i];
					if (!item.IsStatic)
					{
						htFieldValue[item.Name] = item.GetValue(_oSource);
					}
				}
				PropertyInfo[] alProperties = oTp.GetProperties();
				PropertyInfo[] array2 = alProperties;
				for (int i = 0; i < array2.Length; i++)
				{
					PropertyInfo item2 = array2[i];
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
					if (field.FieldType == typeof(int))
					{
						oValue = TypeUtil.ParseInt(string.Concat(_oValue), 0);
					}
					else
					{
						if (field.FieldType == typeof(string))
						{
							oValue = string.Concat(_oValue);
						}
						else
						{
							if (field.FieldType == typeof(double))
							{
								oValue = TypeUtil.ParseDouble(string.Concat(_oValue), 0.0);
							}
							else
							{
								if (field.FieldType == typeof(long))
								{
									oValue = TypeUtil.ParseLong(string.Concat(_oValue), 0L);
								}
							}
						}
					}
					field.SetValue(_oSource, oValue);
				}
			}
		}
	}
}
