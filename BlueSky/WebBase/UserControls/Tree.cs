using System;
using System.Collections.Generic;
using System.Text;

namespace WebBase.UserControls
{
    public abstract class Tree<T> : ITree<T> where T : class, ITree<T>
    {
        public TreeNode RootNode;
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
            TreeMeta.Meta.TextFild = t.GetProperty(TreeMeta.TextFieldName);
            if (null == TreeMeta.Meta.TextFild)
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
        public abstract List<T> List();
        public abstract List<T> List(object _LinkStartValue);
        public virtual void InitRootNode()
        {
            List<T> lt = this.List();
        }

    }
}
