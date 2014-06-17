using System;

namespace BlueSky.Utilities
{
    public class StringUtil
    {
        public static string FixLegth(string _strSource, int _nLength)
        {
            if (string.IsNullOrEmpty(_strSource))
                return _strSource;
            return _strSource.Length <= _nLength ? _strSource : (_strSource.Substring(0, _nLength) + "...");
        }
    }
}
