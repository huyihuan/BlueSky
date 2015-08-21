using System;
using System.Collections.Generic;
using System.Text;

namespace WebBase.UserControls
{
    public class Tree<T> where T : class, ITree<T>
    {
        public TreeNode RootNode;
        public bool IsDisplayRootNode;
        public List<TreeNode> Nodes;
        public static TreeAttribute TreeMeta;
        static Tree()
        {
            Type t = typeof(T);
            TreeAttribute[] treeAttributes = (TreeAttribute[])t.GetCustomAttributes(typeof(TreeAttribute), false);
            if (null == treeAttributes || treeAttributes.Length == 0)
            {
                throw new Exception(string.Format("Type {0} unrealized TreeAttribute features.", t.FullName));
            }
            TreeMeta = treeAttributes[0];
            string strProertyNullOrEmptyFormat = "The property {0} " + string.Format("of type {0} TreeAttribute features is NullOrEmpty.", t.FullName);
            string strPropertyNotFoundFormat = "The property \"{0}\" of type \"{1}\" was not found.";
            if (string.IsNullOrEmpty(TreeMeta.TextFieldName))
            {
                throw new Exception(string.Format(strProertyNullOrEmptyFormat, "TextFieldName"));
            }
            if (string.IsNullOrEmpty(TreeMeta.ValueFieldName))
            {
                throw new Exception(string.Format(strProertyNullOrEmptyFormat, "ValueFieldName"));
            }
            if (string.IsNullOrEmpty(TreeMeta.LinkFieldName))
            {
                throw new Exception(string.Format(strProertyNullOrEmptyFormat, "LinkFieldName"));
            }
            TreeMeta.Meta.TextField = t.GetProperty(TreeMeta.TextFieldName);
            if (null == TreeMeta.Meta.TextField)
            {
                throw new Exception(string.Format(strPropertyNotFoundFormat, TreeMeta.TextFieldName, t.FullName));
            }
            TreeMeta.Meta.ValueField = t.GetProperty(TreeMeta.ValueFieldName);
            if (null == TreeMeta.Meta.ValueField)
            {
                throw new Exception(string.Format(strPropertyNotFoundFormat, TreeMeta.ValueFieldName, t.FullName));
            }
            TreeMeta.Meta.LinkField = t.GetProperty(TreeMeta.LinkFieldName);
            if (null == TreeMeta.Meta.LinkField)
            {
                throw new Exception(string.Format(strPropertyNotFoundFormat, TreeMeta.LinkFieldName, t.FullName));
            }
        }
        public Tree()
        {
            this.Nodes = new List<TreeNode>();
        }
        public virtual void InitNodes(List<T> _ltInit)
        {
            //初始化TreeNode
            List<T> lt = null != _ltInit ? _ltInit : (this as ITree<T>).TreeList();
            if (null == lt || lt.Count == 0)
                return;
            TypeCode LinkTCode = Type.GetTypeCode(TreeMeta.LinkStartValue.GetType());
            bool bInt = LinkTCode == TypeCode.Int32 || LinkTCode == TypeCode.Int16 || LinkTCode == TypeCode.Int64;
            foreach (T t in lt)
            {
                object oValue = TreeMeta.Meta.LinkField.GetValue(t, null);
                if (bInt ? ((int)oValue == (int)TreeMeta.LinkStartValue) : ((oValue + "") == (TreeMeta.LinkStartValue + "")))
                {
                    TreeNode node = new TreeNode();
                    node.Text = TreeMeta.Meta.TextField.GetValue(t, null) + "";
                    node.Value = TreeMeta.Meta.ValueField.GetValue(t, null) + "";
                    node.Data = t.TreeNodeData;
                    this.Nodes.Add(node);
                    this.LoadChildrenNodes(node, lt);
                }
            }
            if (this.IsDisplayRootNode && null != this.RootNode && this.Nodes.Count >= 1)
            {
                this.RootNode.Nodes = new List<TreeNode>(this.Nodes);
            }
        }
        protected void LoadChildrenNodes(TreeNode _ParentNode, List<T> _ltAllNodes) 
        {
            foreach (T t in _ltAllNodes)
            {
                object oValue = TreeMeta.Meta.LinkField.GetValue(t, null);
                if ((oValue + "") == (_ParentNode.Value + ""))
                {
                    TreeNode node = new TreeNode();
                    node.Text = TreeMeta.Meta.TextField.GetValue(t, null) + "";
                    node.Value = TreeMeta.Meta.ValueField.GetValue(t, null) + "";
                    node.Data = t.TreeNodeData;
                    _ParentNode.Nodes.Add(node);
                    LoadChildrenNodes(node, _ltAllNodes);
                }
            }
        }
        
    }
}
