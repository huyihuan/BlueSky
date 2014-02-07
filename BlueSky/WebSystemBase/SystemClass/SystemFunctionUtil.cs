using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Text;
using DataBase;
using System.Web.UI.WebControls;
using WebSystemBase.Utilities;

namespace WebSystemBase.SystemClass
{
    public class SystemFunctionUtil
    {
        #region 功能树样式名称

        //tree-class:
        public const string tree = "treelist";
        public const string node = "tree-node";
        public const string node_box = "tree-nodes-box";

        public const string node_head_normal = "tree-node-head-normal";
        public const string node_head_active = "tree-node_head_active";
        public const string node_head_icon_leaf = "tree-node-head-icon-leaf";
        public const string node_head_icon_parent = "tree-node-folder-normal";
        public const string node_head_text = "tree-node-head-text";

        public const string node_dot_plus = "tree-node-dot-plus";
        public const string node_dot_minus = "tree-node-dot-minus";
        public const string node_dot_one_plus = "tree-node-dot-one-plus";
        public const string node_dot_one_minus = "tree-node-dot-one-minus";
        public const string node_dot_first_plus = "tree-node-dot-first-plus";
        public const string node_dot_last_plus = "tree-node-dot-last-plus";
        public const string node_dot_first_minus = "tree-node-dot-first-minus";
        public const string node_dot_last_minus = "tree-node-dot-last-minus";

        public const string node_branch_line = "tree-node-branch-line";
        public const string node_branch_middle = "tree-node-branch-middle";
        public const string node_branch_last = "tree-node-branch-last";
        public const string node_branch_first = "tree-node-branch-first";
        public const string node_branch_empty = "tree-node-branch-empty";
        public const string node_branch_common = "tree-node-branch-common";
        public const string node_branch_one = "tree-node-branch-one";

        public const string node_type_parent = "node-parent";
        public const string node_type_son = "node-son";

        #endregion

        public const string Target_Top = "top";
        public const string Target_WorkFrame = "workframe";

        public const string NodeType_Parent = "parent";
        public const string NodeType_Son = "son";

        public static void BindTargetList(ListControl __ltControl)
        {
            __ltControl.Items.Clear();
            __ltControl.Items.Add(Target_Top);
            __ltControl.Items.Add(Target_WorkFrame);
        }

        private static Hashtable htParentIdToCount = new Hashtable();
        private static Hashtable htIdToProperty = new Hashtable();
        private static Hashtable htIdToFunction = new Hashtable();
        private static SystemFunction[] Functions = null;
        public static string CreateFunctionTree()
        {
            int nCurrentUserId = SystemUtil.GetCurrentUserId();
            Functions = SystemFunction.GetUserFunctin(nCurrentUserId);
            if (null == Functions || Functions.Length == 0)
                return "";
            htParentIdToCount = SystemFunction.GetParentIdToCount(Functions);
            foreach (SystemFunction funcItem in Functions)
                htIdToFunction[funcItem.Id] = funcItem;

            int nFunctionsCount = Functions.Length;
            int nRootIndex = 1;
            StringBuilder sbTree = new StringBuilder();
            for (int i = 0; i < nFunctionsCount; i++)
            {
                if (Functions[i].ParentId == -1)
                {
                    sbTree.Append(__CreateTree(Functions[i], nRootIndex));
                    nRootIndex++;
                }
            }
            return string.Format("<div class='{0}'><div style='height:5px;'></div>{1}</div>", tree, sbTree.ToString());
        }
        /*
         * tree结构
         * 节点id命名规则：tree_n1_n1_...,tree_n1_n2_....,tree_n2_n1_....
         * 节点的子节点div容器id命名规则为：父节点id + "nodes";
         */
        private static string __CreateTree(SystemFunction __FunctionNode, int __NodeIndex)
        {
            int nSonCount = Util.ParseInt(htParentIdToCount[__FunctionNode.Id] + "", 0);
            int nFriendCount = Util.ParseInt(htParentIdToCount[__FunctionNode.ParentId] + "", 0);
            Hashtable ht = new Hashtable();
            ht["index"] = __NodeIndex;
            ht["friend"] = nFriendCount;
            ht["son"] = nSonCount;
            htIdToProperty[__FunctionNode.Id] = ht;
            string strNode = __CreateNode(__FunctionNode);
            if (nSonCount > 0)
            {
                int nNodeIndex = 1;
                int nLength = Functions.Length;
                StringBuilder sbNodes = new StringBuilder();
                for (int i = 0; i < nLength; i++)
                {
                    if (Functions[i].ParentId == __FunctionNode.Id)
                    {
                        sbNodes.Append(__CreateTree(Functions[i], nNodeIndex));
                        nNodeIndex++;
                    }
                }
                strNode = strNode + string.Format("<div class='{0}'>{1}</div>", node_box, sbNodes.ToString());
            }
            return strNode;
        }

        private static string __CreateNode(SystemFunction function)
        {
            string strText = function.Name;
            //string strUrl = "Window.aspx?functionid=" + function.Id;
            string strTip = function.Description;

            Hashtable ht = (Hashtable)htIdToProperty[function.Id];
            int nFriendsCount = Util.ParseInt(ht["friend"] + "", 0);
            int nIndex = Util.ParseInt(ht["index"] + "", 0);
            int nLevel = function.Level;
            StringBuilder sbNode = new StringBuilder();

            int nParentId = function.ParentId;
            for (int i = 0; i < nLevel - 1; i++)
            {
                if (nParentId != -1)
                {
                    SystemFunction parentFunction = (SystemFunction)htIdToFunction[nParentId];
                    Hashtable htParent = (Hashtable)htIdToProperty[parentFunction.Id];
                    int nPCount = Util.ParseInt(htParent["friend"] + "", 0);
                    int nPIndex = Util.ParseInt(htParent["index"] + "", 0);
                    bool bLast = nPCount == nPIndex;
                    string strBranchClassName = bLast ? node_branch_empty : node_branch_line;
                    sbNode.Insert(0, __CreateSpan(SystemFunctionUtil.node_branch_common + " " + strBranchClassName, ""));
                    nParentId = parentFunction.ParentId;
                }
            }
            int nChildrenCount = Util.ParseInt(htParentIdToCount[function.Id] + "", -1);

            if (nChildrenCount > 0)//父节点
            {
                string strDot = SystemFunctionUtil.node_dot_plus;
                if (nLevel == 1 && nFriendsCount == 1)
                {
                    strDot = SystemFunctionUtil.node_dot_one_plus;
                }
                else if (nLevel == 1 && nFriendsCount > 1 && nIndex == 1)
                {
                    strDot = SystemFunctionUtil.node_dot_first_plus;
                }
                else if (nIndex == nFriendsCount)
                {
                    strDot = SystemFunctionUtil.node_dot_last_plus;
                }
                sbNode.Append(__CreateCommonSpan(SystemFunctionUtil.node_branch_common + " " + strDot, "_type=" + NodeType_Parent, true));

                //添加叶子
                string strNodeIcon = __CreateSpan(SystemFunctionUtil.node_head_icon_parent, "");
                string strNodeText = __CreateSpan(SystemFunctionUtil.node_head_text, strText);
                sbNode.Append(__CreateSpan(SystemFunctionUtil.node_head_normal, strNodeIcon + strNodeText, __GetTagString(function)));
            }
            else //子节点
            {
                string strDot = SystemFunctionUtil.node_branch_middle;
                if (nLevel == 1 && nFriendsCount == 1)
                {
                    strDot = SystemFunctionUtil.node_branch_one;
                }
                else if (nLevel == 1 && nFriendsCount > 1 && nIndex == 1)
                {
                    strDot = SystemFunctionUtil.node_branch_first;
                }
                else if (nIndex == nFriendsCount)
                {
                    strDot = SystemFunctionUtil.node_branch_last;
                }
                sbNode.Append(__CreateCommonSpan(SystemFunctionUtil.node_branch_common + " " + strDot, "", false));

                //添加叶子
                string strNodeIcon = __CreateSpan(SystemFunctionUtil.node_head_icon_leaf, "");
                string strNodeText = __CreateSpan(SystemFunctionUtil.node_head_text, string.Format("<a href='#'>{0}</a>", strText));
                sbNode.Append(__CreateSpan(SystemFunctionUtil.node_head_normal, strNodeIcon + strNodeText, __GetTagString(function)));
            }
            string strNode = string.Format("<div class='{0} {1}'>{2}</div>", SystemFunctionUtil.node, nChildrenCount > 0 ? SystemFunctionUtil.node_type_parent : SystemFunctionUtil.node_type_son, sbNode.ToString());
            return strNode;
        }

        private static string __CreateSpan(string __ClassName, string __Content)
        {
            return string.Format("<span class=\"{0}\">{1}</span>", __ClassName, __Content);
        }

        private static string __CreateSpan(string __ClassName, string __Content, string __NodeTag)
        {
            return string.Format("<span class=\"{0}\" onclick=\"nodePlusMinusExchange(this);nodeClick(this);\"{1}>{2}</span>", __ClassName, __NodeTag, __Content);
        }

        private static string __CreateCommonSpan(string __ClassName, string __NodeTag, bool __bClick)
        {
            string strClick = __bClick ? " onclick=\"nodePlusMinusExchange(this);nodeClick(this);\"" : "";
            return string.Format("<span class=\"{0}\" {1}{2}></span>", __ClassName, __NodeTag, strClick);
        }

        private static string __GetTagString(SystemFunction _FuncObj)
        {
            if (null == _FuncObj)
                return "";
            int nChildrenCount = Util.ParseInt(htParentIdToCount[_FuncObj.Id] + "", -1);
            string strNodeType = nChildrenCount > 0 ? SystemFunctionUtil.NodeType_Parent : SystemFunctionUtil.NodeType_Son;
            StringBuilder sbTag = new StringBuilder();
            sbTag.Append(string.Format(" _tag=\"{0}\"", _FuncObj.Id));
            sbTag.Append(string.Format(" _type=\"{0}\"", strNodeType));
            sbTag.Append(string.Format(" _name=\"{0}\"", _FuncObj.Name.Replace("\"", "").Replace("'", "")));
            sbTag.Append(string.Format(" _value=\"{0}\"", _FuncObj.Id));
            sbTag.Append(string.Format(" _icon=\"{0}\"", _FuncObj.IconName));
            //sbTag.Append(string.Format(" _target=\"{0}\"", _FuncObj.Target));
            //sbTag.Append(string.Format(" _width=\"{0}\"", _FuncObj.Width));
            //sbTag.Append(string.Format(" _height=\"{0}\"", _FuncObj.Height));
            //sbTag.Append(string.Format(" _resize=\"{0}\"", _FuncObj.IsResize));
            //sbTag.Append(string.Format(" _move=\"{0}\"", _FuncObj.IsToMove));
            //sbTag.Append(string.Format(" _minbox=\"{0}\"", _FuncObj.IsIncludeMinBox));
            //sbTag.Append(string.Format(" _maxbox=\"{0}\"", _FuncObj.IsIncludeMaxBox));
            //sbTag.Append(string.Format(" _intaskbar=\"{0}\"", _FuncObj.IsShowInTaskBar));
            return sbTag.ToString();
        }
    }
}
