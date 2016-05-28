using BlueSky.Interfaces;
using System;
namespace BlueSky.Cache
{
	public class CacheItem<TValue> : ICacheItem<TValue>
	{
		public TValue Value
		{
			get;
			set;
		}
		public DateTime ExpirationTime
		{
			get;
			set;
		}
		public CacheItem()
		{
		}
		public CacheItem(TValue _Value, int _nExpirationSeconds)
		{
			this.Value = _Value;
			this.ExpirationTime = DateTime.Now.AddSeconds((double)_nExpirationSeconds);
		}
	}
}
