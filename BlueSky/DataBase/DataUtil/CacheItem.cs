using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace DataBase
{
    public class CacheItem
    {
        //缓存有效时间
        static TimeSpan Ts = new TimeSpan(0, 20, 0);

        static string strKeyHeader = "BlueSky_";
        static string strValueCacheKey = "cache_bluesky_value";
        static string strTimeCacheKey = "cache_bluesky_time";
        static Hashtable htCache = new Hashtable();
        static Hashtable htCacheTime = new Hashtable();

        static CacheItem()
        {
            System.Web.Caching.Cache cache = HttpContext.Current.Cache;
            if (null != cache)
            {
                object oHtValue = cache.Get(strValueCacheKey);
                if (null != oHtValue)
                {
                    htCache = (Hashtable)oHtValue;
                }

                object oHtTime = cache.Get(strTimeCacheKey);
                if (null != oHtTime)
                {
                    htCacheTime = (Hashtable)oHtTime;
                }
            }
        }

        public static object GetValue(string __CacheKey)
        {
            string strKey = __GetKey(__CacheKey);
            object oTs = htCacheTime[strKey];
            if (null == oTs)
                return null;
            if (Ts.Ticks < (DateTime.Now.Ticks - (long)oTs))
                return null;
            object oValue = htCache[strKey];
            return oValue;
        }

        public static void SetValue(string __CacheKey, object __oValue)
        {
            string strKey = __GetKey(__CacheKey);
            htCache[strKey] = __oValue;
            htCacheTime[strKey] = DateTime.Now.Ticks;
        }

        private static string __GetKey(string __SourceKey)
        {
            if (__SourceKey.IndexOf(strKeyHeader) >= 0)
                return __SourceKey;
            else
                return strKeyHeader + __SourceKey;
        }

        public static void Clear()
        {
            if (null == htCache)
                return;
            htCache.Clear();
            htCacheTime.Clear();
        }

        public static void Delete(string __CacheKey)
        {
            string strKey = __GetKey(__CacheKey);
            htCache.Remove(strKey);
            htCacheTime.Remove(strKey);
        }

        public static bool IsExistCache(string __CacheKey)
        {
            return null != htCache[__CacheKey];
        }
    }

    
}
