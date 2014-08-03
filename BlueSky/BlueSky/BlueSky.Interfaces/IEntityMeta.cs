using System;
namespace BlueSky.Interfaces
{
	public interface IEntityMeta<TEntity> where TEntity : IEntity
	{
		Type EntityType
		{
			get;
		}
		string EntityName
		{
			get;
		}
		bool EnableCache
		{
			get;
			set;
		}
		IEntityField KeyField
		{
			get;
			set;
		}
		IEntityField[] EntityFields
		{
			get;
			set;
		}
		string Selects
		{
			get;
			set;
		}
		object KeyValue(TEntity _Entity);
	}
}
