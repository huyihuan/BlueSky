using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Cache;
using BlueSky.Interfaces;

namespace BlueSky.Cache
{
    internal class CacheItem : CacheBase, ICacheItem
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public long lTsExpiration { get; set; }
        public long dtCacheTime { get; set; }

        public CacheItem() 
        {
            this.dtCacheTime = DateTime.Now.Ticks;
        }
        public CacheItem(string _strKey)
        {
            this.Key = FormatCacheKey(_strKey);
            lTsExpiration = CacheBase.lTsExpiration;
            this.dtCacheTime = DateTime.Now.Ticks;
        }
        public CacheItem(string _strKey, long _lTsExpiration)
        {
            this.Key = FormatCacheKey(_strKey);
            this.lTsExpiration = _lTsExpiration;
            this.dtCacheTime = DateTime.Now.Ticks;
        }
        public CacheItem(string _strKey, object _oValue, long _lTsExpiration)
        {
            this.Key = FormatCacheKey(_strKey);
            this.Value = _oValue;
            this.lTsExpiration = _lTsExpiration;
            this.dtCacheTime = DateTime.Now.Ticks;
        }
        public string FormatCacheKey(string _strKey)
        {
            return CacheBase.FormatCacheKey(_strKey);
        }
    }
}
