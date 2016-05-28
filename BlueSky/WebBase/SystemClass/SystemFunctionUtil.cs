using BlueSky.Utilities;
using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using WebBase.Utilities;
namespace WebBase.SystemClass
{
	public class SystemFunctionUtil
	{
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
		public const string Target_Top = "top";
		public const string Target_WorkFrame = "workframe";
		public const string NodeType_Parent = "parent";
		public const string NodeType_Son = "son";
		private static Hashtable htParentIdToCount = new Hashtable();
		private static Hashtable htIdToProperty = new Hashtable();
		private static Hashtable htIdToFunction = new Hashtable();
		private static SystemFunction[] Functions = null;
		public static void BindTargetList(ListControl __ltControl)
		{
			__ltControl.Items.Clear();
			__ltControl.Items.Add("top");
			__ltControl.Items.Add("workframe");
		}
		public static string CreateFunctionTree()
		{
			int nCurrentUserId = SystemUtil.GetCurrentUserId();
			SystemFunctionUtil.Functions = SystemFunction.GetUserFunctin(nCurrentUserId);
			string result;
			if (SystemFunctionUtil.Functions == null || SystemFunctionUtil.Functions.Length == 0)
			{
				result = "";
			}
			else
			{
				SystemFunctionUtil.htParentIdToCount = SystemFunction.GetParentIdToCount(SystemFunctionUtil.Functions);
				SystemFunction[] functions = SystemFunctionUtil.Functions;
				for (int j = 0; j < functions.Length; j++)
				{
					SystemFunction funcItem = functions[j];
					SystemFunctionUtil.htIdToFunction[funcItem.Id] = funcItem;
				}
				int nFunctionsCount = SystemFunctionUtil.Functions.Length;
				int nRootIndex = 1;
				StringBuilder sbTree = new StringBuilder();
				for (int i = 0; i < nFunctionsCount; i++)
				{
					if (SystemFunctionUtil.Functions[i].ParentId == -1)
					{
						sbTree.Append(SystemFunctionUtil.__CreateTree(SystemFunctionUtil.Functions[i], nRootIndex));
						nRootIndex++;
					}
				}
				result = string.Format("<div class='{0}'><div style='height:5px;'></div>{1}</div>", "treelist", sbTree.ToString());
			}
			return result;
		}
		private static string __CreateTree(SystemFunction __FunctionNode, int __NodeIndex)
		{
			int nSonCount = TypeUtil.ParseInt(string.Concat(SystemFunctionUtil.htParentIdToCount[__FunctionNode.Id]), 0);
			int nFriendCount = TypeUtil.ParseInt(string.Concat(SystemFunctionUtil.htParentIdToCount[__FunctionNode.ParentId]), 0);
			Hashtable ht = new Hashtable();
			ht["index"] = __NodeIndex;
			ht["friend"] = nFriendCount;
			ht["son"] = nSonCount;
			SystemFunctionUtil.htIdToProperty[__FunctionNode.Id] = ht;
			string strNode = SystemFunctionUtil.__CreateNode(__FunctionNode);
			if (nSonCount > 0)
			{
				int nNodeIndex = 1;
				int nLength = SystemFunctionUtil.Functions.Length;
				StringBuilder sbNodes = new StringBuilder();
				for (int i = 0; i < nLength; i++)
				{
					if (SystemFunctionUtil.Functions[i].ParentId == __FunctionNode.Id)
					{
						sbNodes.Append(SystemFunctionUtil.__CreateTree(SystemFunctionUtil.Functions[i], nNodeIndex));
						nNodeIndex++;
					}
				}
				strNode += string.Format("<div class='{0}'>{1}</div>", "tree-nodes-box", sbNodes.ToString());
			}
			return strNode;
		}
		private static string __CreateNode(SystemFunction function)
		{
			string strText = function.Name;
			string strTip = function.Description;
			Hashtable ht = (Hashtable)SystemFunctionUtil.htIdToProperty[function.Id];
			int nFriendsCount = TypeUtil.ParseInt(string.Concat(ht["friend"]), 0);
			int nIndex = TypeUtil.ParseInt(string.Concat(ht["index"]), 0);
			int nLevel = function.Level;
			StringBuilder sbNode = new StringBuilder();
			int nParentId = function.ParentId;
			for (int i = 0; i < nLevel - 1; i++)
			{
				if (nParentId != -1)
				{
					SystemFunction parentFunction = (SystemFunction)SystemFunctionUtil.htIdToFunction[nParentId];
					Hashtable htParent = (Hashtable)SystemFunctionUtil.htIdToProperty[parentFunction.Id];
					int nPCount = TypeUtil.ParseInt(string.Concat(htParent["friend"]), 0);
					int nPIndex = TypeUtil.ParseInt(string.Concat(htParent["index"]), 0);
					string strBranchClassName = (nPCount == nPIndex) ? "tree-node-branch-empty" : "tree-node-branch-line";
					sbNode.Insert(0, SystemFunctionUtil.__CreateSpan("tree-node-branch-common " + strBranchClassName, ""));
					nParentId = parentFunction.ParentId;
				}
			}
			int nChildrenCount = TypeUtil.ParseInt(string.Concat(SystemFunctionUtil.htParentIdToCount[function.Id]), -1);
			if (nChildrenCount > 0)
			{
				string strDot = "tree-node-dot-plus";
				if (nLevel == 1 && nFriendsCount == 1)
				{
					strDot = "tree-node-dot-one-plus";
				}
				else
				{
					if (nLevel == 1 && nFriendsCount > 1 && nIndex == 1)
					{
						strDot = "tree-node-dot-first-plus";
					}
					else
					{
						if (nIndex == nFriendsCount)
						{
							strDot = "tree-node-dot-last-plus";
						}
					}
				}
				sbNode.Append(SystemFunctionUtil.__CreateCommonSpan("tree-node-branch-common " + strDot, "_type=parent", true));
				string strNodeIcon = SystemFunctionUtil.__CreateSpan("tree-node-folder-normal", "");
				string strNodeText = SystemFunctionUtil.__CreateSpan("tree-node-head-text", strText);
				sbNode.Append(SystemFunctionUtil.__CreateSpan("tree-node-head-normal", strNodeIcon + strNodeText, SystemFunctionUtil.__GetTagString(function)));
			}
			else
			{
				string strDot = "tree-node-branch-middle";
				if (nLevel == 1 && nFriendsCount == 1)
				{
					strDot = "tree-node-branch-one";
				}
				else
				{
					if (nLevel == 1 && nFriendsCount > 1 && nIndex == 1)
					{
						strDot = "tree-node-branch-first";
					}
					else
					{
						if (nIndex == nFriendsCount)
						{
							strDot = "tree-node-branch-last";
						}
					}
				}
				sbNode.Append(SystemFunctionUtil.__CreateCommonSpan("tree-node-branch-common " + strDot, "", false));
				string strNodeIcon = SystemFunctionUtil.__CreateSpan("tree-node-head-icon-leaf", "");
				string strNodeText = SystemFunctionUtil.__CreateSpan("tree-node-head-text", string.Format("<a href='#'>{0}</a>", strText));
				sbNode.Append(SystemFunctionUtil.__CreateSpan("tree-node-head-normal", strNodeIcon + strNodeText, SystemFunctionUtil.__GetTagString(function)));
			}
			return string.Format("<div class='{0} {1}'>{2}</div>", "tree-node", (nChildrenCount > 0) ? "node-parent" : "node-son", sbNode.ToString());
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
			string result;
			if (null == _FuncObj)
			{
				result = "";
			}
			else
			{
				int nChildrenCount = TypeUtil.ParseInt(string.Concat(SystemFunctionUtil.htParentIdToCount[_FuncObj.Id]), -1);
				string strNodeType = (nChildrenCount > 0) ? "parent" : "son";
				StringBuilder sbTag = new StringBuilder();
				sbTag.Append(string.Format(" _tag=\"{0}\"", _FuncObj.Id));
				sbTag.Append(string.Format(" _type=\"{0}\"", strNodeType));
				sbTag.Append(string.Format(" _name=\"{0}\"", _FuncObj.Name.Replace("\"", "").Replace("'", "")));
				sbTag.Append(string.Format(" _value=\"{0}\"", _FuncObj.Id));
				sbTag.Append(string.Format(" _icon=\"{0}\"", _FuncObj.IconName));
				result = sbTag.ToString();
			}
			return result;
		}
	}
}
