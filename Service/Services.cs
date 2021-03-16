using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Service
{
    public static class Services
    {
        static Services()
        {
            _serLs = new List<ServiceBase>();
            _map = new Dictionary<Type, Type>();
        }

        private static List<ServiceBase> _serLs;
        private static Dictionary<Type, Type> _map;//映射关系

        public static T GetService<T>() where T : ServiceBase
        {
            var ser = _serLs.OfType<T>().FirstOrDefault<T>();
            if (ser == null)
            {
                ser = CreateService<T>();
            }
            return ser;
        }

        private static T CreateService<T>() where T : ServiceBase
        {
            var ctor = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            return ctor[0].Invoke(new object[] { }) as T;
        }

        private static bool Register<K, V>()
        {
            var kType = typeof(K);
            var vType = typeof(V);

            if (_map.ContainsKey(kType))
            {
                Log.Waring("重复注册");
                return false;
            }
            else
            {
                _map.Add(kType, vType);
                return true;
            }
        }
    }
}