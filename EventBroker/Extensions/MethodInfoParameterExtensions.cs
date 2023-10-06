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

        public static bool HasEqualParameterTypesTo(this MethodInfo method, IEnumerable<object> passedObjects)
        {
            Type[] requiredTypes = method.GetMethodParameterTypes().ToArray();
            Type[] passedTypes = passedObjects.GetObjectTypes()
                                              .ToArray();
            return requiredTypes.ContentEqual(passedTypes);
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
