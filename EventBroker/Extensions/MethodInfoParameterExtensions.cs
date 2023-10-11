using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker.Extensions
{
    internal static class MethodInfoParameterExtensions
    {
        public static bool HasParameters(this MethodInfo method)
        {
            return method.GetParameters().Length > 0;
        }

        public static bool ArePassableToMethod(this IEnumerable<object> passedObjects, MethodInfo receiver)
        {
            IEnumerable<Type> requiredTypes = receiver.GetMethodParameterTypes();
            IEnumerable<Type> passedTypes = passedObjects.GetObjectTypes();
            return passedTypes.AreLengthEqualAndPassableTo(requiredTypes);
        }

        public static bool AreLengthEqualAndPassableTo(this IEnumerable<Type> passedTypes, IEnumerable<Type> requiredTypes)
        {
            if (passedTypes.Count() == requiredTypes.Count())
            {
                return passedTypes.ArePassableTo(requiredTypes);
            }
            else
            {
                return false;
            }
        }

        private static bool ArePassableTo(this IEnumerable<Type> passedTypes, IEnumerable<Type> requiredTypes)
        {
            bool passable = true;
            for (int i = 0; i < requiredTypes.Count(); i++)
            {
                passable = passable && requiredTypes.ElementAt(i).IsAssignableFrom(passedTypes.ElementAt(i));
            }
            return passable;
        }

        public static IEnumerable<Type> GetMethodParameterTypes(this MethodInfo method)
        {
            ParameterInfo[] methodParameters = method.GetParameters()
                                                      .ToArray();
            Type[] methodParameterTypes = methodParameters.GetParameterTypes()
                                                     .ToArray();
            return methodParameterTypes;
        }
    }
}
