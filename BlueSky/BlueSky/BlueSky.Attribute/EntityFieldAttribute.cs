using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueSky.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class EntityFieldAttribute: System.Attribute
    {
        private string _FieldName;
        private bool _IsPrimaryKey = false;
        private string _FieldType;
        public string FieldName 
        {
            get 
            {
                return this._FieldName;
            }
            set
            {
                this._FieldName = value;
            }
        }
        public bool IsPrimaryKey
        {
            get
            {
                return this._IsPrimaryKey;
            }
            set
            {
                this._IsPrimaryKey = value;
            }
        }
        public string FieldType
        {
            get
            {
                return this._FieldType;
            }
            set
            {
                this._FieldType = value;
            }
        }
        

        public EntityFieldAttribute()
        { }
        public EntityFieldAttribute(string _FieldName)
        {
            this.FieldName = _FieldName;
        }
        public EntityFieldAttribute(string _FieldName, bool _IsPrimaryKey)
        {
            this.FieldName = _FieldName;
            this.IsPrimaryKey = _IsPrimaryKey;
        }
    }
}
