using System;
namespace BlueSky.Utilities
{
	public class StringUtil
	{
		public static string FixLegth(string _strSource, int _nLength)
		{
			string result;
			if (string.IsNullOrEmpty(_strSource))
			{
				result = _strSource;
			}
			else
			{
				result = ((_strSource.Length <= _nLength) ? _strSource : (_strSource.Substring(0, _nLength) + "..."));
			}
			return result;
		}
	}
}
