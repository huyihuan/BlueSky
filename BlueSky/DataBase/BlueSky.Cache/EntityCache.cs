using System;
using System.Collections.Generic;
using BlueSky.Utilities;
using BlueSky.Interfaces;

namespace BlueSky.Cache
{
    internal class EntityCache : CacheBase
    {
        private static new long lTsExpiration;
        private static new string KeyHeader;
        static EntityCache()
        {
            string strMinutes = System.Configuration.ConfigurationManager.AppSettings["EntityCacheExpirationTime"];
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
            KeyHeader = CacheBase.FormatCacheKey("EntityCache");
        }

        public static object GetEntity(string _strEntityName,object _oKey)
        {
            CacheItem oCache = CacheBase.GetCache<CacheItem>(string.Format("{0}_{1}_{2}", KeyHeader, _strEntityName, _oKey));
            return null == oCache ? null : oCache.Value;
        }
        public static void SetEntity(string _strEntityName, object _oKey, object _oEntity)
        {
            SetEntity(_strEntityName, _oKey, _oEntity, lTsExpiration);
        }
        public static void SetEntity(string _strEntityName, object _oKey, object _oEntity, long _lExpirationTicks)
        {
            CacheItem cache = new CacheItem(string.Format("{0}_{1}_{2}", KeyHeader, _strEntityName, _oKey), _oEntity, _lExpirationTicks);
            CacheBase.SetCache<CacheItem>(cache);
        }
        public static void SetEntity(string _strEntityName, object _oKey, object _oEntity, int _nExpirationSecond)
        {
            TimeSpan TsExpiration = new TimeSpan(0, 0, _nExpirationSecond);
            SetEntity(_strEntityName, _oKey, _oEntity, TsExpiration.Ticks);
        }

        public static void Clear(string _strEntityName, object _oKey)
        {
            CacheBase.Clear(string.Format("{0}_{1}_{2}", KeyHeader, _strEntityName, _oKey));
        }
        public new static void Clear(string _strEntityName)
        {
            CacheBase.ClearContainsKey(string.Format("{0}_{1}", KeyHeader, _strEntityName));
        }
        public static bool IsExistEntity(string _strEntityName, object _oKey)
        {
            return CacheBase.IsExistCache(string.Format("{0}_{1}_{2}", KeyHeader, _strEntityName, _oKey));
        }
    }
}
