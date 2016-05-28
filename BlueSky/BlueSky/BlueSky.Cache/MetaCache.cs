using BlueSky.Interfaces;
using BlueSky.Utilities;
using System;
using System.Configuration;
namespace BlueSky.Cache
{
	internal static class MetaCache<TEntity> where TEntity : IEntity
	{
		private static int Expiration;
		private static string KeyHeader;
		static MetaCache()
		{
			string strMinutes = ConfigurationManager.AppSettings["MetaCacheExpirationTime"];
			int nMinutes = TypeUtil.ParseInt(strMinutes, -1);
			MetaCache<TEntity>.Expiration = ((nMinutes <= 0) ? 20 : nMinutes) * 60;
			MetaCache<TEntity>.KeyHeader = Cache.FormatKey("MetaCache");
		}
		public static IEntityMeta<TEntity> Find()
		{
			CacheItem<IEntityMeta<TEntity>> oCache = Cache<IEntityMeta<TEntity>>.Find(MetaCache<TEntity>.FormatKey());
			return (oCache == null) ? null : oCache.Value;
		}
		public static void Add(IEntityMeta<TEntity> _Meta, int _nExpirationSeconds)
		{
			Cache<IEntityMeta<TEntity>>.Add(MetaCache<TEntity>.FormatKey(), _Meta, _nExpirationSeconds);
		}
		public static void Add(IEntityMeta<TEntity> _Meta)
		{
			Cache<IEntityMeta<TEntity>>.Add(MetaCache<TEntity>.FormatKey(), _Meta, MetaCache<TEntity>.Expiration);
		}
		public static bool Exist()
		{
			return Cache<IEntityMeta<TEntity>>.Exist(MetaCache<TEntity>.FormatKey());
		}
		public static string FormatKey()
		{
			return string.Format("{0}[{1}][Schema]", MetaCache<TEntity>.KeyHeader, typeof(TEntity).Name);
		}
	}
}
