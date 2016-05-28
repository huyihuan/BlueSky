using BlueSky.Interfaces;
using BlueSky.Utilities;
using System;
using System.Configuration;
namespace BlueSky.Cache
{
	internal static class EntityListCache<TEntity> where TEntity : IEntity
	{
		private static int Expiration;
		private static string KeyHeader;
		static EntityListCache()
		{
			string strMinutes = ConfigurationManager.AppSettings["EntityListCacheExpirationTime"];
			int nMinutes = TypeUtil.ParseInt(strMinutes, -1);
			EntityListCache<TEntity>.Expiration = ((nMinutes <= 0) ? 20 : nMinutes) * 60;
			EntityListCache<TEntity>.KeyHeader = Cache.FormatKey("EntityListCache");
		}
		public static TEntity[] Find()
		{
			return EntityListCache<TEntity>.Find("");
		}
		public static TEntity[] Find(string _strFilter)
		{
			CacheItem<TEntity[]> oCache = Cache<TEntity[]>.Find(EntityListCache<TEntity>.FormatKey(_strFilter));
			return (oCache == null) ? null : oCache.Value;
		}
		public static void Add(TEntity[] _EntityList)
		{
			EntityListCache<TEntity>.Add("", _EntityList);
		}
		public static void Add(TEntity[] _EntityList, int _nExpirationSeconds)
		{
			EntityListCache<TEntity>.Add("", _EntityList, _nExpirationSeconds);
		}
		public static void Add(string _strFilter, TEntity[] _EntityList)
		{
			EntityListCache<TEntity>.Add(_strFilter, _EntityList, EntityListCache<TEntity>.Expiration);
		}
		public static void Add(string _strFilter, TEntity[] _EntityList, int _nExpirationSeconds)
		{
			Cache<TEntity[]>.Add(EntityListCache<TEntity>.FormatKey(_strFilter), _EntityList, _nExpirationSeconds);
		}
		public static void AddCount(int _nCount)
		{
			EntityListCache<TEntity>.AddCount("", _nCount);
		}
		public static void AddCount(string _strFilter, int _nCount)
		{
			Cache<int>.Add(EntityListCache<TEntity>.FormatCountKey(_strFilter), _nCount, EntityListCache<TEntity>.Expiration);
		}
		public static int FindCount(string _strFilter)
		{
			CacheItem<int> oCache = Cache<int>.Find(EntityListCache<TEntity>.FormatCountKey(_strFilter));
			return (oCache == null) ? -1 : oCache.Value;
		}
		public static int FindCount()
		{
			return EntityListCache<TEntity>.FindCount("");
		}
		public static void Clear()
		{
			Cache<TEntity[]>.Clear();
		}
		public static void ClearCount()
		{
			Cache<int>.Clear(EntityListCache<TEntity>.FormatCountKey(""));
		}
		public static void Remove(string _strFilter)
		{
			Cache<TEntity[]>.Remove(EntityListCache<TEntity>.FormatKey(_strFilter));
		}
		public static bool Exist()
		{
			return EntityListCache<TEntity>.Exist("");
		}
		public static bool Exist(string _strFilter)
		{
			return Cache<TEntity[]>.Exist(EntityListCache<TEntity>.FormatKey(_strFilter));
		}
		public static bool ExistCount()
		{
			return EntityListCache<TEntity>.ExistCount("");
		}
		public static bool ExistCount(string _strFilter)
		{
			return Cache<int>.Exist(EntityListCache<TEntity>.FormatCountKey(_strFilter));
		}
		public static string FormatKey(string _strFilter)
		{
			if (!string.IsNullOrEmpty(_strFilter))
			{
				_strFilter = string.Format("[{0}]", _strFilter);
			}
			return string.Format("{0}[{1}][List]{2}", EntityListCache<TEntity>.KeyHeader, typeof(TEntity).Name, _strFilter);
		}
		public static string FormatCountKey(string _strFilter)
		{
			if (!string.IsNullOrEmpty(_strFilter))
			{
				_strFilter = string.Format("[{0}]", _strFilter);
			}
			return string.Format("{0}[{1}][Count]{2}", EntityListCache<TEntity>.KeyHeader, typeof(TEntity).Name, _strFilter);
		}
	}
}
