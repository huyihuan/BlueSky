using System;
using System.Collections;
namespace BlueSky.Cache
{
	public static class CacheInfomation
	{
		public static Hashtable EntityCacheInformation()
		{
			return null;
		}
		public static Hashtable ListCacheInformation()
		{
			return null;
		}
		public static void ClearCache()
		{
			CacheInfomation.ClearEntityCache();
			CacheInfomation.ClearListCache();
		}
		public static void ClearEntityCache()
		{
		}
		public static void ClearListCache()
		{
		}
	}
}
