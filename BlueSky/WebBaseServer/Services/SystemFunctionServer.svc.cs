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
            SystemFunction[] list = SystemFunction.GetUserFunctin(_nUserId);
            if (null == list || list.Length == 0)
                return "";
            TreeNode root = new TreeNode("系统功能", "");
            foreach (SystemFunction oFunction in list)
            {
                if (oFunction.ParentId <= 0)
                {
                    TreeNode node = new TreeNode(oFunction.Name, oFunction.Id);
                    GetChildrenFunction(node, list);
                    root.Nodes.Add(node);
                }
            }
            return root.ToJSON();
        }

        private void GetChildrenFunction(TreeNode _parentNode, SystemFunction[] _Functions)
        {
            foreach (SystemFunction oFunction in _Functions)
            {
                if (oFunction.ParentId == (int)_parentNode.Value)
                {
                    TreeNode node = new TreeNode(oFunction.Name, oFunction.Id);
                    GetChildrenFunction(node, _Functions);
                    _parentNode.Nodes.Add(node);
                }
            }
        }
    }
}
