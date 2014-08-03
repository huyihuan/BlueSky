using System;
using BlueSky.Interfaces;
using System.Reflection;
using System.Collections.Generic;
using BlueSky.Attribute;

namespace BlueSky.EntityAccess
{
    public class EntityMeta<TEntity> : IEntityMeta<TEntity> where TEntity : IEntity, new()
    {
        public Type EntityType
        {
            get
            {
                return typeof(TEntity);
            }
        }
        public string EntityName
        {
            get
            {
                return this.EntityType.Name;
            }
        }
        public bool EnableCache { get; set; }
        public IEntityField KeyField { get; set; }
        public IEntityField[] EntityFields { get; set; }
        public string Selects { get; set; }
        public EntityMeta()
        {
            this.EnableCache = true;
            object[] alEAttributes = this.EntityType.GetCustomAttributes(typeof(EntityAttribue), true);
            if (null != alEAttributes && alEAttributes.Length >= 1)
            {
                this.EnableCache = ((EntityAttribue)alEAttributes[0]).EnableCache;
            }
            PropertyInfo[] alProperties = this.EntityType.GetProperties();
            List<EntityField> ltFields = new List<EntityField>();
            List<string> ltSelect = new List<string>();
            foreach (PropertyInfo oProperty in alProperties)
            {
                if (oProperty.CanWrite)
                {
                    EntityField eField = new EntityField();
                    eField.FieldName = oProperty.Name;
                    eField.Meta = oProperty;
                    eField.Type = oProperty.PropertyType;
                    EntityFieldAttribute EFAttribute = (EntityFieldAttribute)System.Attribute.GetCustomAttribute(oProperty, typeof(EntityFieldAttribute));
                    if (null != EFAttribute)
                    {
                        if (EFAttribute.IsPrimaryKey)
                            this.KeyField = eField;
                        if (!string.IsNullOrEmpty(EFAttribute.FieldName))
                            eField.FieldName = EFAttribute.FieldName;
                    }
                    ltFields.Add(eField);
                    ltSelect.Add(string.Format("[{0}]", eField.FieldName));
                }
            }
            this.EntityFields = ltFields.ToArray();
            this.Selects = string.Join(",", ltSelect.ToArray());
        }
        public object KeyValue(TEntity _Entity)
        {
            if (null == this.KeyField)
                return null;
            return KeyField.FieldValue(_Entity);
        }
    }
}
