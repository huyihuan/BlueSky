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
using WebBase.UserControls;

namespace WebBaseServer.Services
{
    public class SystemFunctionServer : ServerProcessBase, ISystemFunctionServer
    {
        public string GetUserFunction(int _nUserId)
        {
            SystemFunction tree = new SystemFunction();
            tree.RootNode = new TreeNode("系统功能", "");
            tree.IsDisplayRootNode = true;
            tree.InitNodes(new List<SystemFunction>(SystemFunction.GetUserFunctin(_nUserId)));
            return tree.RootNode.ToJSON();
        }

    }
}
