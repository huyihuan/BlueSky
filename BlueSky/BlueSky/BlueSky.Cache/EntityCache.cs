using BlueSky.Interfaces;
using BlueSky.Utilities;
using System;
using System.Configuration;
namespace BlueSky.Cache
{
	public static class EntityCache<TEntity> where TEntity : IEntity
	{
		private static int Expiration;
		private static string KeyHeader;
		static EntityCache()
		{
			string strMinutes = ConfigurationManager.AppSettings["EntityCacheExpirationTime"];
			int nMinutes = TypeUtil.ParseInt(strMinutes, -1);
			EntityCache<TEntity>.Expiration = ((nMinutes <= 0) ? 20 : nMinutes) * 60;
			EntityCache<TEntity>.KeyHeader = Cache.FormatKey("EntityCache");
		}
		public static TEntity Find(object _oKey)
		{
			CacheItem<TEntity> oCache = Cache<TEntity>.Find(EntityCache<TEntity>.FormatKey(_oKey));
			return (oCache == null) ? default(TEntity) : oCache.Value;
		}
		public static void Add(object _oKey, TEntity _oEntity)
		{
			EntityCache<TEntity>.Add(_oKey, _oEntity, EntityCache<TEntity>.Expiration);
		}
		public static void Add(object _oKey, TEntity _oEntity, int _nExpirationSeconds)
		{
			Cache<TEntity>.Add(EntityCache<TEntity>.FormatKey(_oKey), _oEntity, _nExpirationSeconds);
		}
		public static void Remove(object _oKey)
		{
			Cache<TEntity>.Remove(EntityCache<TEntity>.FormatKey(_oKey));
		}
		public static bool Exist(object _oKey)
		{
			return Cache<TEntity>.Exist(EntityCache<TEntity>.FormatKey(_oKey));
		}
		public static string FormatKey(object _oKey)
		{
			return string.Format("{0}[{1}][{2}]", EntityCache<TEntity>.KeyHeader, typeof(TEntity).Name, _oKey);
		}
	}
}
