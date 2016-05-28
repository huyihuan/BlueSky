using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Utilities;

namespace BlueSky.Cache
{
    internal class SchemaCache : CacheBase
    {
        private static new long lTsExpiration;
        private static new string KeyHeader;
        static SchemaCache()
        {
            string strMinutes = System.Configuration.ConfigurationManager.AppSettings["SchemaCacheExpirationTime"];
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
            KeyHeader = CacheBase.FormatCacheKey("SchemaCache");
        }
        public static object GetSchema(string _strEntityName)
        {
            CacheItem oCache = CacheBase.GetCache<CacheItem>(string.Format("{0}_{1}", KeyHeader, _strEntityName));
            return null == oCache ? null : oCache.Value;
        }
        public static void SetSchema(string _strEntityName, object _oSchema)
        {
            SetSchema(_strEntityName, _oSchema, lTsExpiration);
        }
        public static void SetSchema(string _strEntityName, object _oSchema, long _lTsExpiration)
        {
            CacheItem cache = new CacheItem(string.Format("{0}_{1}", KeyHeader, _strEntityName), _oSchema, _lTsExpiration);
            CacheBase.SetCache<CacheItem>(cache);
        }
        public static bool IsExistSchema(string _strEntityName)
        {
            return CacheBase.IsExistCache(string.Format("{0}_{1}", KeyHeader, _strEntityName));
        }
    }
}
