using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker.Extensions
{
    internal static class EnumerableToEnumerableOfTypeExtensions
    {
        public static IEnumerable<Type> GetObjectTypes<T>(this IEnumerable<T> values)
        {
            return values.Select((obj) => obj.GetType());
        }

        public static IEnumerable<Type> GetParameterTypes(this IEnumerable<ParameterInfo> parameters)
        {
            return parameters.Select((parameter) => parameter.ParameterType);
        }
    }
}
