using System;
using System.Reflection;
using BlueSky.Interfaces;
using BlueSky.Utilities;

namespace BlueSky.EntityAccess
{
    public class EntityField : IEntityField
    {
        private string  _FieldName;
        private Type _Type;
        private PropertyInfo _Meta;
        private TypeCode _TypeCode;
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
        public TypeCode TypeCode
        {
            get
            {
                return this._TypeCode;
            }
            set
            {
                this._TypeCode = value;
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
            if (this.TypeCode == TypeCode.Int32 && !(_oValue is Int32))
            {
                if (_oValue == null || _oValue is DBNull)
                {
                    _oValue = 0;
                }
                else
                {
                    _oValue = TypeUtil.ParseInt(_oValue.ToString(), 0);
                }
            }
            else if (this.TypeCode == TypeCode.Double && !(_oValue is Double))
            {
                if (_oValue == null || _oValue is DBNull)
                {
                    _oValue = 0d;
                }
                else
                {
                    _oValue = TypeUtil.ParseDouble(_oValue.ToString(), 0d);
                }
            }
            else if (this.TypeCode == TypeCode.Int64 && !(_oValue is Int64))
            {
                if (_oValue == null || _oValue is DBNull)
                {
                    _oValue = 0L;
                }
                else
                {
                    _oValue = TypeUtil.ParseLong(_oValue.ToString(), 0L);
                }
            }
            this.Meta.SetValue(_Entity, Convert.ChangeType(_oValue, this.Type), null);
        }
    }
}
