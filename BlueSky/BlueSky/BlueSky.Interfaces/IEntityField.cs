using System;
using System.Reflection;
namespace BlueSky.Interfaces
{
	public interface IEntityField
	{
		string FieldName
		{
			get;
			set;
		}
		Type Type
		{
			get;
			set;
		}
		PropertyInfo Meta
		{
			get;
			set;
		}
        TypeCode TypeCode { get; set; }
		object FieldValue<TEntity>(TEntity _Entity) where TEntity : IEntity;
		void SetValue<TEntity>(TEntity _Entity, object _oValue) where TEntity : IEntity;
	}
}
