using System.Reflection;

namespace EventBroker.MethodInstances
{
    internal static partial class MethodInstanceFactory
    {
        private class MethodInstanceWithoutParameters : MethodInstance
        {
            public MethodInstanceWithoutParameters(object instance, MethodInfo method) : base(instance, method)
            {
            }

            public override void Invoke(object[] parameters)
            {
                Method.Invoke(Instance, null);
            }
        }
    }
}