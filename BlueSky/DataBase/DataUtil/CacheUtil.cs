using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace DataBase
{
    public class CacheUtil
    {
        //缓存有效时间（默认的缓存时间为20分钟）
        static long lTsExpiration = 0;

        //数据缓存key的头部标识
        static string strKeyHeader = "BlueSky_";

        static string strCacheValueKey = "BlueSky_CacheValueKey";
        static string strCacheTimeKey = "BlueSky_CacheTimeKey";
        static string strCacheExpirationKey = "BlueSky_CacheExpirationKey";
        static Hashtable htCache = new Hashtable();
        static Hashtable htCacheTime = new Hashtable();
        static Hashtable htCustomerExpiration = new Hashtable();

        static CacheUtil()
        {
            //恢复应用程序缓存
            System.Web.Caching.Cache appCache = HttpContext.Current.Cache;
            if (null != appCache)
            {
                object oCache = appCache.Get(strCacheValueKey);
                if (null != oCache)
                    htCache = (Hashtable)oCache;
                object oCacheTime = appCache.Get(strCacheTimeKey);
                if (null != oCacheTime)
                    htCacheTime = (Hashtable)oCacheTime;
                object oCahceExpiration = appCache.Get(strCacheExpirationKey);
                if (null != oCahceExpiration)
                    htCustomerExpiration = (Hashtable)oCahceExpiration;
            }
            //获取用户自定义的缓存过期时间"ExpirationTime"（单位：分钟，默认值：20）
            string strMinutes = System.Configuration.ConfigurationManager.AppSettings["ExpirationTime"];
            int nExpirationMinutes = Util.ParseInt(strMinutes, -1);
            if (nExpirationMinutes <= 0)
                nExpirationMinutes = 20;
            TimeSpan tsTemp = new TimeSpan(0, nExpirationMinutes, 0);
            lTsExpiration = tsTemp.Ticks;
        }

        public static object GetValue(string _CacheKey)
        {
            string strKey = GetCustomerKey(_CacheKey);
            object oTs = htCacheTime[strKey];
            if (null == oTs)
                return null;
            long lCustomerExpiration = null == htCustomerExpiration[strKey] ? lTsExpiration : (long)htCustomerExpiration[strKey];
            if (lCustomerExpiration < (DateTime.Now.Ticks - (long)oTs))
            {
                Clear(strKey);
                return null;
            }
            return htCache[strKey];
        }

        public static void SetValue(string _CacheKey, object _oValue)
        {
            string strKey = GetCustomerKey(_CacheKey);
            htCache[strKey] = _oValue;
            htCacheTime[strKey] = DateTime.Now.Ticks;
        }

        public static void SetValue(string _CacheKey, object _oValue, long _ExpirationTicks)
        {
            string strKey = GetCustomerKey(_CacheKey);
            htCache[strKey] = _oValue;
            htCacheTime[strKey] = DateTime.Now.Ticks;
            htCustomerExpiration[strKey] = _ExpirationTicks;
        }

        public static void SetValue(string _CacheKey, object _oValue, int _nExpirationSecond)
        {
            TimeSpan TsExpiration = new TimeSpan(0, 0, _nExpirationSecond);
            SetValue(_CacheKey, _oValue, TsExpiration.Ticks);
        }

        private static string GetCustomerKey(string _SourceKey)
        {
            if (_SourceKey.IndexOf(strKeyHeader) >= 0)
                return _SourceKey;
            else
                return strKeyHeader + _SourceKey;
        }

        public static void ClearAll()
        {
            if (null == htCache)
                return;
            lock (htCache)
            {
                htCache.Clear();
            }
            lock (htCacheTime)
            {
                htCacheTime.Clear();
            }
            lock (htCustomerExpiration)
            {
                htCustomerExpiration.Clear();
            }
        }

        public static void Clear(string _CacheKey)
        {
            string strKey = GetCustomerKey(_CacheKey);
            lock (htCache)
            {
                htCache.Remove(strKey);
            }
            lock (htCacheTime)
            {
                htCacheTime.Remove(strKey);
            }
            lock (htCustomerExpiration)
            {
                htCustomerExpiration.Remove(strKey);
            }
        }

        public static void ClearContainsKey(string _CacheKeyPart)
        { 
            if(null == htCache || htCache.Count == 0)
                return;
            Hashtable htCacheTemp = (Hashtable)htCache.Clone();
            foreach (string strKey in htCacheTemp.Keys)
            {
                if (strKey.IndexOf(_CacheKeyPart) >= 0)
                    Clear(strKey);
            }
        }

        public static bool IsExistCache(string _CacheKey)
        {
            string strKey = GetCustomerKey(_CacheKey);
            return null != htCache[strKey];
        }
    }

    
}
