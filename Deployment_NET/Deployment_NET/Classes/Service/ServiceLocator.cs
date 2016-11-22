using System;
using System.Collections.Generic;
using System.Linq;

namespace awinta.Deployment_NET.Service
{

    public sealed class ServiceLocator
    {

        private static List<object> data = new List<object>();

        private ServiceLocator() { }

        public static void Add<T>(T Value)
        {
            data.Add(Value);
        }

        public static T GetInstance<T>()
        {

            Type type = typeof(T);

            return (T)data.FirstOrDefault(x => x.GetType() == type);
        }

        public static IEnumerable<T> GetInstances<T>()
        {

            Type type = typeof(T);

            var query = from Element in data
                        where Element.GetType() == type
                        select (T)Element;

            return query;

        }

        public static void Remove<T>()
        {

            Type type = typeof(T);

            data.RemoveAll(x => x.GetType() == type);

        }

        public static void Destroy()
        {

            data = null;

        }

    }
}
