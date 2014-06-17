using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSky.Interfaces
{
    public interface ICacheItem
    {
        string Key{get;set;}
        object Value { get; set; }
        long lTsExpiration { get; set; }
        long dtCacheTime { get; set; }
        string FormatCacheKey(string _strKey);
    }
}
