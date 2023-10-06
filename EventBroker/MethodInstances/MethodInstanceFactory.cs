using EventBroker.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker.MethodInstances
{
    internal static class MethodInstanceFactory
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

        private class MethodInstanceWithParameters : MethodInstance
        {
            public MethodInstanceWithParameters(object instance, MethodInfo method) : base(instance, method)
            {
            }

            public override void Invoke(object[] parameters)
            {
                if (Method.HasEqualParameterTypesTo(parameters))
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
