using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace BlueSky.Utilities
{
    public class ReflectionUtil
    {
        public static string[] GetObjectFieldsList(object _oSource, bool _bIncludeProperty)
        {
            if (null == _oSource)
                return null;
            List<string> ltReturnList = new List<string>();
            Type oTp = _oSource.GetType();
            FieldInfo[] alFields = oTp.GetFields();
            foreach (FieldInfo item in alFields)
            {
                if (item.IsStatic)
                    continue;
                ltReturnList.Add(item.Name);
            }
            if (_bIncludeProperty)
            {
                PropertyInfo[] alProperties = oTp.GetProperties();
                foreach (PropertyInfo item in alProperties)
                    ltReturnList.Add(item.Name);
            }
            return ltReturnList.ToArray();
        }

        public static Hashtable GetObjectFieldValueHash(object _oSource)
        {
            if (null == _oSource)
                return null;
            Hashtable htFieldValue = new Hashtable();
            Type oTp = _oSource.GetType();
            FieldInfo[] alFields = oTp.GetFields();
            foreach (FieldInfo item in alFields)
            {
                if (item.IsStatic)
                    continue;
                htFieldValue[item.Name] = item.GetValue(_oSource);
            }
            PropertyInfo[] alProperties = oTp.GetProperties();
            foreach (PropertyInfo item in alProperties)
                htFieldValue[item.Name] = item.GetValue(_oSource, null);
            return htFieldValue;
        }

        public static object GetObjectFieldValue(object _oSource, string _strFieldName)
        {
            if (null == _oSource || string.IsNullOrEmpty(_strFieldName))
                return null;
            Type oTp = _oSource.GetType();
            FieldInfo field = oTp.GetField(_strFieldName);
            if (null == field)
                return null;
            return field.GetValue(_oSource);
        }

        public static void SetObjectFieldValue(object _oSource, string _strFieldName, object _oValue)
        {
            if (null == _oSource || string.IsNullOrEmpty(_strFieldName) || null == _oValue)
                return;
            Type oTp = _oSource.GetType();
            FieldInfo field = oTp.GetField(_strFieldName);
            if (null == field)
                return;
            object oValue = new object();
            if (field.FieldType == typeof(int))
                oValue = TypeUtil.ParseInt(_oValue + "", 0);
            else if (field.FieldType == typeof(string))
                oValue = _oValue + "";
            else if (field.FieldType == typeof(double))
                oValue = TypeUtil.ParseDouble(_oValue + "", 0d);
            else if (field.FieldType == typeof(long))
                oValue = TypeUtil.ParseLong(_oValue + "", 0L);

            field.SetValue(_oSource, oValue);
        }
    }
}
