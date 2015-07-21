using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WebBase.Interface
{
    public abstract class ServerProcessBase
    {
        public virtual string Json(Hashtable _htJson, bool _End)
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
                sbJson.Append(JsonKeyValue(strKey, _htJson[strKey]));
            }
            string strContent = (sbJson.Length >= 1) ? sbJson.ToString() : "";
            if (_End)
            {
                return JsonEnd(strContent);
            }
            return strContent;
        }

        public virtual string JsonKeyValue(string _Key, object _Value)
        {
            return string.Format("\"{0}\" : {1}", _Key, _Value);
        }

        public string JsonEnd(string _JsonContent)
        {
            return "{" + _JsonContent + "}";
        }
    }
}
