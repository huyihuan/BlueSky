using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;

namespace BlueSky.BlueSky.Utilities
{
    public class JSON
    {
        public string Json(Hashtable _htJson, bool _End)
        {
            if (null == _htJson || _htJson.Count == 0)
                return "";
            StringBuilder sbJson = new StringBuilder();
            int nFlag = 0;
            foreach (string strKey in _htJson.Keys)
            {
                if (nFlag != 0)
                {
                    sbJson.Append(" ,");
                }
                sbJson.Append(JsonKeyValue(strKey, _htJson[strKey], false));
                nFlag++;
            }
            string strContent = (sbJson.Length >= 1) ? sbJson.ToString() : "";
            if (_End)
            {
                return JsonEnd(strContent);
            }
            return strContent;
        }

        public string JsonKeyValue(string _Key, object _Value, bool _ValueIsArrayOrObject)
        {
            TypeCode code = Type.GetTypeCode(_Value.GetType());
            string jsonFormat = "\"{0}\" : {1}";
            if (!_ValueIsArrayOrObject && (code == TypeCode.String || code == TypeCode.DateTime || code == TypeCode.Char))
            {
                jsonFormat = "\"{0}\" : \"{1}\"";
            }
            return string.Format(jsonFormat, _Key, _Value == null ? "" : _Value);
        }

        public string JsonEnd(string _JsonContent)
        {
            return ("{" + _JsonContent + "}");
        }

        public string Json2Array(string _JsonContent)
        {
            return ("[" + _JsonContent + "]");
        }
    }
}
