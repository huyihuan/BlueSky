using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Reflection;
using System.Collections;

namespace DataBase
{
    public class Util
    {
        public static int ParseInt(string _strSource, int _iDefault)
        {
            int nReturnValue = 0;
            bool bParse = int.TryParse(_strSource, out nReturnValue);
            return bParse ? nReturnValue : _iDefault;
        }

        public static double ParseDouble(string _strSource, double _dDefault)
        {
            double dReturnValue = 0;
            bool bParse = double.TryParse(_strSource, out dReturnValue);
            return bParse ? dReturnValue : _dDefault;
        }

        public static double ParseLong(string _strSource, long _lDefault)
        {
            long lReturnValue = 0L;
            bool bParse = long.TryParse(_strSource, out lReturnValue);
            return bParse ? lReturnValue : _lDefault;
        }

        public static string MD5Encrypt(string _strSource)
        {
            if(string.IsNullOrEmpty(_strSource))
                return "";
            System.Security.Cryptography.MD5 md5Factory = System.Security.Cryptography.MD5.Create();
            byte[] byteSource = Encoding.Default.GetBytes(_strSource);
            byte[] byteMd5 = md5Factory.ComputeHash(byteSource);
            string strResult = BitConverter.ToString(byteMd5).Replace("-", "");
            return strResult;
        }

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
                oValue = Util.ParseInt(_oValue + "", 0);
            else if (field.FieldType == typeof(string))
                oValue = _oValue + "";
            else if (field.FieldType == typeof(double))
                oValue = Util.ParseDouble(_oValue + "", 0d);
            else if (field.FieldType == typeof(long))
                oValue = Util.ParseLong(_oValue + "", 0L);
                     
            field.SetValue(_oSource, oValue);
        }

    }
}
