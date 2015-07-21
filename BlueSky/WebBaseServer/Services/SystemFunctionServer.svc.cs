using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebBaseServer.Interface;
using WebBase.SystemClass;
using System.Collections;
using WebBase.Interface;
using System.ServiceModel.Activation;

namespace WebBaseServer.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SystemFunctionServer : ServerProcessBase, ISystemFunctionServer
    {
        public string GetUserFunction(int _nUserId)
        {
            SystemFunction[] list = SystemFunction.GetUserFunctin(_nUserId);
            if (null == list || list.Length == 0)
                return "";
            StringBuilder sbJson = new StringBuilder();
            foreach (SystemFunction oFunction in list)
            {
                if (oFunction.ParentId <= 0)
                {
                    StringBuilder sbOne = new StringBuilder();
                    Hashtable ht = new Hashtable();
                    ht["text"] = oFunction.Name;
                    ht["value"] = oFunction.Id;
                    sbOne.Append(base.Json(ht, false));
                    sbOne.Append(base.JsonKeyValue("childrens", GetChildrenFunction(oFunction.Id, list)));
                    if (sbJson.Length >= 1)
                    {
                        sbJson.Append(",");
                    }
                    sbJson.Append(base.JsonEnd(sbOne.ToString()));
                }
            }
            return sbJson.ToString();
        }

        private string GetChildrenFunction(int _nParentId, SystemFunction[] _Functions)
        {
            StringBuilder sbJson = new StringBuilder();
            foreach (SystemFunction oFunction in _Functions)
            {
                if (oFunction.ParentId == _nParentId)
                {
                    StringBuilder sbChildren = new StringBuilder();
                    Hashtable ht = new Hashtable();
                    ht["text"] = oFunction.Name;
                    ht["value"] = oFunction.Id;
                    sbChildren.Append(base.Json(ht, false));
                    string strChildren = GetChildrenFunction(oFunction.Id, _Functions);
                    if (!string.IsNullOrEmpty(strChildren))
                    {
                        sbChildren.Append(base.JsonKeyValue("childrens", strChildren));
                    }
                    if (sbJson.Length >= 1)
                    {
                        sbJson.Append(",");
                    }
                    sbJson.Append(base.JsonEnd(sbChildren.ToString()));
                }
            }
            return string.Format("[{0}]", sbJson.ToString());
        }
    }
}
