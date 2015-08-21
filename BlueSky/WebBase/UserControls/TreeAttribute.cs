using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace WebBase.UserControls
{
    public class TreeAttribute : Attribute
    {
        public string TextFieldName { get; set; }
        public string ValueFieldName { get; set; }
        public string LinkFieldName { get; set; }
        public object LinkStartValue { get; set; }
        public TreeAttribute()
        {
            //默认初始LinkStartValue为-1
            this.LinkStartValue = -1;
            this.Meta = new TreeMeta();
        }
        protected internal TreeMeta Meta { get; set; }
    }

    public class TreeMeta
    {
        public PropertyInfo TextField { get; set; }
        public PropertyInfo ValueField { get; set; }
        public PropertyInfo LinkField { get; set; }
    }
}
