using System;
using System.Reflection;
using BlueSky.Interfaces;

namespace BlueSky.EntityAccess
{
    public class EntityField : IEntityField
    {
        private string  _FieldName;
        private Type _Type;
        private PropertyInfo _Meta;
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
        public Type Type 
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
            }
        }
        public PropertyInfo Meta 
        {
            get
            {
                return this._Meta;
            }
            set
            {
                this._Meta = value;
            }
        }
        public object FieldValue<TEntity>(TEntity _Entity) where TEntity : IEntity
        {
            return this.Meta.GetValue(_Entity, null);
        }
        public void SetValue<TEntity>(TEntity _Entity,object _oValue) where TEntity : IEntity
        {
            this.Meta.SetValue(_Entity, Convert.ChangeType(_oValue, this.Type), null);
        }
    }
}
