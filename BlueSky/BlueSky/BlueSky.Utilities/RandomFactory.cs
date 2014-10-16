using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueSky.Utilities
{
    public class RandomFactory
    {
        private static readonly Random R;
        static RandomFactory()
        {
            R = new Random(DateTime.Now.Millisecond);
        }
        public static int GetInteger()
        {
            return R.Next();
        }
        public static int GetInteger(int _maxValue)
        {
            return R.Next(_maxValue);
        }
        public static int GetInteger(int _minValue, int _maxValue)
        {
            return R.Next(_minValue, _maxValue);
        }
        public static double GetDouble()
        {
            return R.NextDouble();
        }
    }
}
