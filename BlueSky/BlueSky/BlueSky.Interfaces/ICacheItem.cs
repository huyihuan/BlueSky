using System;
namespace BlueSky.Interfaces
{
	public interface ICacheItem<TValue>
	{
		TValue Value
		{
			get;
			set;
		}
		DateTime ExpirationTime
		{
			get;
			set;
		}
	}
}
