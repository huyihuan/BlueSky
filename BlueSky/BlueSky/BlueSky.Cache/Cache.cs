using BlueSky.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
namespace BlueSky.Cache
{
	public class Cache
	{
		private static int Expiration;
		private static string KeyHeader;
		static Cache()
		{
			Cache.Expiration = 0;
			Cache.KeyHeader = "BlueSky&";
			string strMinutes = ConfigurationManager.AppSettings["CacheExpirationTime"];
			int nMinutes = TypeUtil.ParseInt(strMinutes, -1);
			Cache.Expiration = ((nMinutes <= 0) ? 20 : nMinutes) * 60;
		}
		public static string FormatKey(string _strKey)
		{
			return _strKey.StartsWith(Cache.KeyHeader) ? _strKey : (Cache.KeyHeader + _strKey);
		}
	}
	public class Cache<TValue> : Cache
	{
		private static Dictionary<string, CacheItem<TValue>> dicCache
		{
			get;
			set;
		}
		static Cache()
		{
			if (null == Cache<TValue>.dicCache)
			{
				Cache<TValue>.dicCache = new Dictionary<string, CacheItem<TValue>>();
			}
		}
		public static CacheItem<TValue> Find(string _strKey)
		{
			_strKey = Cache.FormatKey(_strKey);
			CacheItem<TValue> result;
			if (!Cache<TValue>.dicCache.ContainsKey(_strKey))
			{
				result = null;
			}
			else
			{
				CacheItem<TValue> oCache = Cache<TValue>.dicCache[_strKey];
				if (DateTime.Now >= oCache.ExpirationTime)
				{
					Cache<TValue>.Remove(_strKey);
					result = null;
				}
				else
				{
					result = oCache;
				}
			}
			return result;
		}
		public static void Add(string _strKey, TValue _Value, int _nExpirationSeconds)
		{
			if (!string.IsNullOrEmpty(_strKey))
			{
				CacheItem<TValue> oCache = Cache<TValue>.Find(_strKey);
				if (null == oCache)
				{
					oCache = new CacheItem<TValue>(_Value, _nExpirationSeconds);
					Cache<TValue>.dicCache.Add(_strKey, oCache);
				}
				else
				{
					oCache.ExpirationTime = DateTime.Now.AddSeconds((double)_nExpirationSeconds);
					oCache.Value = _Value;
				}
			}
		}
		public static void Add(string _strKey, TValue _Value)
		{
			Cache<TValue>.Add(_strKey, _Value, 0);
		}
		public static void Update(string _strKey, int _nExpirationSeconds)
		{
			if (!string.IsNullOrEmpty(_strKey))
			{
				if (Cache<TValue>.dicCache.ContainsKey(_strKey))
				{
					CacheItem<TValue> oCache = Cache<TValue>.dicCache[_strKey];
					if (null != oCache)
					{
						oCache.ExpirationTime = DateTime.Now.AddSeconds((double)_nExpirationSeconds);
					}
				}
			}
		}
		public static void Clear()
		{
			if (Cache<TValue>.dicCache != null && Cache<TValue>.dicCache.Count != 0)
			{
				Dictionary<string, CacheItem<TValue>> dicCache;
				Monitor.Enter(dicCache = Cache<TValue>.dicCache);
				try
				{
					Cache<TValue>.dicCache.Clear();
				}
				finally
				{
					Monitor.Exit(dicCache);
				}
			}
		}
		public static void Remove(string _strKey)
		{
			_strKey = Cache.FormatKey(_strKey);
			Dictionary<string, CacheItem<TValue>> dicCache;
			Monitor.Enter(dicCache = Cache<TValue>.dicCache);
			try
			{
				Cache<TValue>.dicCache.Remove(_strKey);
			}
			finally
			{
				Monitor.Exit(dicCache);
			}
		}
		public static void Clear(string _strKeyStart)
		{
			if (Cache<TValue>.dicCache != null && Cache<TValue>.dicCache.Count != 0)
			{
				List<string> ltKeys = Cache<TValue>.dicCache.Keys.ToList<string>();
				Dictionary<string, CacheItem<TValue>> dicCache;
				Monitor.Enter(dicCache = Cache<TValue>.dicCache);
				try
				{
					foreach (string strKey in ltKeys)
					{
						if (strKey.StartsWith(_strKeyStart))
						{
							Cache<TValue>.dicCache.Remove(strKey);
						}
					}
				}
				finally
				{
					Monitor.Exit(dicCache);
				}
			}
		}
		public static bool Exist(string _strKey)
		{
            _strKey = Cache.FormatKey(_strKey);
            if (Cache<TValue>.dicCache.ContainsKey(_strKey))
            {
                CacheItem<TValue> oCache = Cache<TValue>.dicCache[_strKey];
                return oCache.ExpirationTime > DateTime.Now;
            }
            return false;
		}
	}
}
