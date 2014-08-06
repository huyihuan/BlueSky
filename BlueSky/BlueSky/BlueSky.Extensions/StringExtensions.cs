using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Utilities;

namespace BlueSky.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string _strValue)
        {
            return string.IsNullOrEmpty(_strValue);
        }
        public static int ToInt(this string _strValue, int _nDefault)
        {
            return TypeUtil.ParseInt(_strValue, _nDefault);
        }
        public static Double ToDouble(this string _strValue, double _dDefault)
        {
            return TypeUtil.ParseDouble(_strValue, _dDefault);
        }
        public static float ToDouble(this string _strValue, long _lDefault)
        {
            return TypeUtil.ParseLong(_strValue, _lDefault);
        }
    }
}