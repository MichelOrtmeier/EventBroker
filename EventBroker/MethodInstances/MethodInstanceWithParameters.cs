using EventBroker.Extensions;
using System.Reflection;

namespace EventBroker.MethodInstances
{
    internal static partial class MethodInstanceFactory
    {
        private class MethodInstanceWithParameters : MethodInstance
        {
            public MethodInstanceWithParameters(object instance, MethodInfo method) : base(instance, method)
            {
            }

            public override void Invoke(object[] parameters)
            {
                if (parameters.ArePassableToMethod(Method))
                {
                    Method.Invoke(Instance, parameters);
                }
                else
                {
                    throw new ArgumentException($"The passed parameters did not match the required parameters of {Method.Name}.");
                }
            }
        }
    }
}