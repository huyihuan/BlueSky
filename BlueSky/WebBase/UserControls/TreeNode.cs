using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using BlueSky.BlueSky.Utilities;

namespace WebBase.UserControls
{
    public class TreeNode
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public object Value { get; set; }
        public object Data { get; set; }
        public List<TreeNode> Nodes { get; set; }
        public int NodesCount
        {
            get
            {
                return this.Nodes.Count;
            }
        }
        public TreeNode()
        {
            this.Nodes = new List<TreeNode>();
        }

        public TreeNode(string _Id)
        {
            this.Id = _Id;
            this.Nodes = new List<TreeNode>();
        }

        public TreeNode(string _Text, object _Value)
        {
            this.Text = _Text;
            this.Value = _Value;
            this.Nodes = new List<TreeNode>();
        }

        public TreeNode(string _Id, string _Text, object _Value)
        {
            this.Id = _Id;
            this.Text = _Text;
            this.Value = _Value;
            this.Nodes = new List<TreeNode>();
        }

        public void AddChildren(TreeNode _Node)
        {
            this.Nodes.Add(_Node);
        }

        public void RemoveChildren(TreeNode _Node)
        {
            this.Nodes.Remove(_Node);
        }

        public string ToJSON()
        {
            StringBuilder builder = new StringBuilder();
            JSON json = new JSON();
            Hashtable ht = new Hashtable();
            ht["text"] = this.Text;
            ht["value"] = this.Value;
            builder.Append(json.Json(ht, false));
            builder.Append(",");
            builder.Append(json.JsonKeyValue("childrens", this.ToChildrenJSON(), true));
            return json.JsonEnd(builder.ToString());
        }

        private string ToChildrenJSON()
        {
            StringBuilder builder = new StringBuilder();
            JSON json = new JSON();
            foreach (TreeNode node in this.Nodes)
            {
                StringBuilder builderNode = new StringBuilder();
                Hashtable ht = new Hashtable();
                ht["text"] = node.Text;
                ht["value"] = node.Value;
                builderNode.Append(json.Json(ht, false));
                builderNode.Append(",");
                builderNode.Append(json.JsonKeyValue("childrens", node.ToChildrenJSON(), true));
                if (builder.Length >= 1)
                {
                    builder.Append(",");
                }
                builder.Append(json.JsonEnd(builderNode.ToString()));
            }
            return json.Json2Array(builder.ToString());
        }

    }
}
