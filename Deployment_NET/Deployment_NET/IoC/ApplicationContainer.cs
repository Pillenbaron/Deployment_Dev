using System;
using System.Collections.Generic;
using System.Reflection;

namespace awinta.Deployment_NET.IoC
{
    internal class ApplicationContainer
    {

        private static readonly IDictionary<Type, Type> types = new Dictionary<Type, Type>();

        private static readonly IDictionary<Type, object> typeInstances = new Dictionary<Type, object>();

        public static void Register<TContract, TImplementation>()
        {
            types[typeof(TContract)] = typeof(TImplementation);
        }

        public static void Register<TContract, TImplementation>(TImplementation instance)
        {
            typeInstances[typeof(TContract)] = instance;
        }

        public static T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }

        public static object GetInstance(Type contract)
        {
            if (typeInstances.ContainsKey(contract))
            {
                return typeInstances[contract];
            }
            else
            {
                var implementation = types[contract];
                var constructor = implementation.GetConstructors()[0];
                var constructorParameters = constructor.GetParameters();

                if (constructorParameters.Length == 0)
                {
                    return Activator.CreateInstance(implementation);
                }

                List<object> parameters = new List<object>(constructorParameters.Length);

                foreach (ParameterInfo parameterInfo in constructorParameters)
                {
                    parameters.Add(GetInstance(parameterInfo.ParameterType));
                }

                return constructor.Invoke(parameters.ToArray());

            }

        }

        public static void Release()
        {

            types.Clear();
            typeInstances.Clear();

        }

    }
}
