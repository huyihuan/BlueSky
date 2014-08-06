using System;
namespace BlueSky.Utilities
{
	public class TypeUtil
	{
		public static int ParseInt(string _strSource, int _iDefault)
		{
			int nReturnValue = 0;
			return int.TryParse(_strSource, out nReturnValue) ? nReturnValue : _iDefault;
		}
		public static double ParseDouble(string _strSource, double _dDefault)
		{
			double dReturnValue = 0.0;
			return double.TryParse(_strSource, out dReturnValue) ? dReturnValue : _dDefault;
		}
		public static float ParseLong(string _strSource, long _lDefault)
		{
			long lReturnValue = 0L;
            return (float)(long.TryParse(_strSource, out lReturnValue) ? lReturnValue : _lDefault);
		}
	}
}
