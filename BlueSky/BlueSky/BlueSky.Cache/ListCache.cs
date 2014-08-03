using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Utilities;

namespace BlueSky.Cache
{
    internal class ListCache : CacheBase
    {
        private static new long lTsExpiration;
        private static new string KeyHeader;
        static ListCache()
        {
            string strMinutes = System.Configuration.ConfigurationManager.AppSettings["ListCacheExpirationTime"];
            int nExpirationMinutes = TypeUtil.ParseInt(strMinutes, 0);
            if (nExpirationMinutes <= 0)
            {
                lTsExpiration = CacheBase.lTsExpiration;
            }
            else
            {
                TimeSpan tsTemp = new TimeSpan(0, nExpirationMinutes, 0);
                lTsExpiration = tsTemp.Ticks;
            }
            KeyHeader = CacheBase.FormatCacheKey("ListCache");
        }

        public static object GetList(string _strEntityName)
        {
            return GetList(_strEntityName, "");
        }
        public static object GetList(string _strEntityName, string _strFilter)
        {
            string strKey = string.Format("{0}_{1}", KeyHeader, _strEntityName);
            if (!string.IsNullOrEmpty(_strFilter))
                strKey += "_" + _strFilter;
            CacheItem oCache = CacheBase.GetCache<CacheItem>(strKey);
            return null == oCache ? null : oCache.Value;
        }
        public static void SetList(string _strEntityName, object _oList)
        {
            SetList(_strEntityName, "", _oList);
        }
        public static void SetList(string _strEntityName, object _oList, long _lTsExpiration)
        {
            SetList(_strEntityName, "", _oList, _lTsExpiration);
        }
        public static void SetList(string _strEntityName,string _strFilter, object _oList)
        {
            SetList(_strEntityName, _strFilter, _oList, lTsExpiration);
        }
        public static void SetList(string _strEntityName, string _strFilter, object _oList, long _lTsExpiration)
        {
            string strKey = string.Format("{0}_{1}", KeyHeader, _strEntityName);
            if (!string.IsNullOrEmpty(_strFilter))
                strKey += "_" + _strFilter;
            CacheItem cache = new CacheItem(strKey, _oList, _lTsExpiration);
            CacheBase.SetCache<CacheItem>(cache);
        }


        public static void SetCount(string _strEntityName, int _nCount)
        {
            SetCount(_strEntityName, "", _nCount);
        }
        public static void SetCount(string _strEntityName, string _strFilter, int _nCount)
        {
            string strKey = string.Format("{0}_{1}_COUNT", KeyHeader, _strEntityName);
            if (!string.IsNullOrEmpty(_strFilter))
                strKey += "_" + _strFilter;
            CacheItem cache = new CacheItem(strKey, _nCount, lTsExpiration);
            CacheBase.SetCache<CacheItem>(cache);
        }
        public static int GetCount(string _strEntityName, string _strFilter)
        {
            string strKey = string.Format("{0}_{1}_COUNT", KeyHeader, _strEntityName);
            if (!string.IsNullOrEmpty(_strFilter))
                strKey += "_" + _strFilter;
            CacheItem oCache = CacheBase.GetCache<CacheItem>(strKey);
            return null == oCache ? -1 : (int)oCache.Value;
        }
        public static int GetCount(string _strEntityName)
        {
            return GetCount(_strEntityName, "");
        }

        public new static void Clear(string _strEntityName)
        {
            CacheBase.ClearContainsKey(string.Format("{0}_{1}", KeyHeader, _strEntityName));
        }
        public static void Clear(string _strEntityName, string _strFilter)
        {
            CacheBase.Clear(string.Format("{0}_{1}_{2}", KeyHeader, _strEntityName, _strFilter));
        }
        public static bool IsExistList(string _strEntityName)
        {
            return IsExistList(_strEntityName, "");
        }
        public static bool IsExistList(string _strEntityName, string _strFilter)
        {
            string strKey = string.Format("{0}_{1}", KeyHeader, _strEntityName);
            if (!string.IsNullOrEmpty(_strFilter))
                strKey += "_" + _strFilter;
            return CacheBase.IsExistCache(strKey);
        }
        public static bool IsExistCount(string _strEntityName)
        {
            return IsExistCount(_strEntityName, "");
        }
        public static bool IsExistCount(string _strEntityName, string _strFilter)
        {
            string strKey = string.Format("{0}_{1}_COUNT", KeyHeader, _strEntityName);
            if (!string.IsNullOrEmpty(_strFilter))
                strKey += "_" + _strFilter;
            return CacheBase.IsExistCache(strKey);
        }
    }
}
