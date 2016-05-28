using System;
using BlueSky.DataAccess;
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
        string TableName { get; set; }
        string ConnectionName { get; set; }
        DatabaseType DbType { get; set; }
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
