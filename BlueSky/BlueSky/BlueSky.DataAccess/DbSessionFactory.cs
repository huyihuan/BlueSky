using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Interfaces;
using System.Reflection;

namespace BlueSky.DataAccess
{
    public static class DbSessionFactory
    {
        private static Dictionary<DatabaseType,  Type> _dicSessionFactory;
        static DbSessionFactory()
        {
            _dicSessionFactory = new Dictionary<DatabaseType, Type>();
            Type tRoot = typeof(DbSession);
            Assembly asb = Assembly.GetAssembly(tRoot);
            Type[] tDbSessions = asb.GetTypes();
            if (null != tDbSessions && tDbSessions.Length >= 1)
            {
                foreach (Type t in tDbSessions)
                {
                    if (t.IsSubclassOf(tRoot))
                    {
                        Activator.CreateInstance(t);
                    }
                }
            }
        }
        public static void Register(DatabaseType _DbType, Type _TSession)
        {
            if (_dicSessionFactory.ContainsKey(_DbType))
            {
                return;
            }
            _dicSessionFactory.Add(_DbType, _TSession);
        }
        public static Type Map(DatabaseType _DbType)
        {
            if (!_dicSessionFactory.ContainsKey(_DbType))
            {
                return typeof(DbSession);
            }
            return _dicSessionFactory[_DbType];
        }
        public static void Clear()
        {
            lock (_dicSessionFactory)
            {
                _dicSessionFactory.Clear();
            }
        }
    }
}
