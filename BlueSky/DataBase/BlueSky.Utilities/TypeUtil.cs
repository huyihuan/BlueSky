using System;

namespace BlueSky.Utilities
{
    public class TypeUtil
    {
        public static int ParseInt(string _strSource, int _iDefault)
        {
            int nReturnValue = 0;
            bool bParse = int.TryParse(_strSource, out nReturnValue);
            return bParse ? nReturnValue : _iDefault;
        }

        public static double ParseDouble(string _strSource, double _dDefault)
        {
            double dReturnValue = 0d;
            bool bParse = double.TryParse(_strSource, out dReturnValue);
            return bParse ? dReturnValue : _dDefault;
        }

        public static double ParseLong(string _strSource, long _lDefault)
        {
            long lReturnValue = 0L;
            bool bParse = long.TryParse(_strSource, out lReturnValue);
            return bParse ? lReturnValue : _lDefault;
        }
    }
}
