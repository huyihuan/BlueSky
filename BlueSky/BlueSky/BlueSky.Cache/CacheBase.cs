using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using BlueSky.Utilities;
using BlueSky.Interfaces;

namespace BlueSky.Cache
{
    internal class CacheBase
    {
        //缓存有效时间（默认的缓存时间为20分钟）
        protected static long lTsExpiration = 0;
        //数据缓存key的头部标识
        protected static string KeyHeader = "BlueSky";
        static Hashtable htCache = new Hashtable();
        static CacheBase()
        {
            //获取用户自定义的缓存过期时间"ExpirationTime"（单位：分钟，默认值：20）
            string strMinutes = System.Configuration.ConfigurationManager.AppSettings["CacheExpirationTime"];
            int nExpirationMinutes = TypeUtil.ParseInt(strMinutes, -1);
            if (nExpirationMinutes <= 0)
                nExpirationMinutes = 20;
            TimeSpan tsTemp = new TimeSpan(0, nExpirationMinutes, 0);
            lTsExpiration = tsTemp.Ticks;
        }

        protected static TCacheItem GetCache<TCacheItem>(string _strKey) where TCacheItem : ICacheItem
        {
            _strKey = FormatCacheKey(_strKey);
            TCacheItem oCache = (TCacheItem)htCache[_strKey];
            if (null == oCache)
                return oCache;
            if ((DateTime.Now.Ticks - oCache.dtCacheTime) > oCache.lTsExpiration)
            {
                Clear(_strKey);
                return default(TCacheItem);
            }
            return oCache;
        }

        protected static void SetCache<TCacheItem>(TCacheItem _CacheItem) where TCacheItem : ICacheItem
        {
            if (null == _CacheItem)
                return;
            htCache[_CacheItem.Key] = _CacheItem;
        }

        protected static string FormatCacheKey(string _strKey)
        {
            if (_strKey.IndexOf(KeyHeader) >= 0)
                return _strKey;
            else
                return string.Format("{0}-{1}", KeyHeader, _strKey);
        }

        protected static void Clear()
        {
            if (null == htCache)
                return;
            lock (htCache)
            {
                htCache.Clear();
            }
        }

        protected static void Clear(string _strKey)
        {
            _strKey = FormatCacheKey(_strKey);
            lock (htCache)
            {
                htCache.Remove(_strKey);
            }
        }

        protected static void Clear<TCacheItem>(TCacheItem _oCache) where TCacheItem : ICacheItem
        {
            if (null == _oCache)
                return;
            lock (htCache)
            {
                htCache.Remove(_oCache.Key);
            }
        }

        protected static void ClearContainsKey(string _strContainsKey)
        { 
            if(null == htCache || htCache.Count == 0)
                return;
            Hashtable htCacheTemp = (Hashtable)htCache.Clone();
            foreach (string strKey in htCacheTemp.Keys)
            {
                if (strKey.IndexOf(_strContainsKey) >= 0)
                    Clear(strKey);
            }
        }

        protected static bool IsExistCache(string _strKey)
        {
            _strKey = FormatCacheKey(_strKey);
            //return null != htCache[strKey]; 排除缓存存在但是值为Null的情况，用Key来查找缓存值即ContainsKey，忽略Value是否为Null
            return htCache.ContainsKey(_strKey);
        }
    }

    
}
