using EventBroker.Extensions;
using System.Reflection;

namespace EventBroker.MethodInstances
{
    internal static partial class MethodInstanceFactory
    {
        public static MethodInstance CreateMethodInstance(object instance, MethodInfo method)
        {
            if (method.HasParameters())
            {
                return new MethodInstanceWithParameters(instance, method);
            }
            else
            {
                return new MethodInstanceWithoutParameters(instance, method);
            }
        }
    }
}
