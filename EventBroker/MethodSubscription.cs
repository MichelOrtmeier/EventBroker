using EventBroker.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker
{
    public class MethodSubscription
    {
        public MethodInfo SubscribingMethod { get; }
        public object Subscriber { get; }

        public MethodSubscription(MethodInfo subscribingMethod, object subscriber)
        {
            SubscribingMethod = subscribingMethod;
            Subscriber = subscriber;
        }

        public void InvokeMethodWithParameters(object[] parameters)
        {
            if (MethodHasEqualParameterTypesTo(parameters.GetObjectTypes()))
            {
                InvokeMethod(parameters);
            }
            else if (!MethodHasParameters())
            {
                InvokeMethod(null);
            }
            else
            {
                throw new ArgumentException($"The compared types of '{SubscribingMethod.Name}'" +
                $"did neither match the given parameters nor had no required parameters");
            }
        }

        private bool MethodHasParameters()
        {
            return SubscribingMethod.GetParameters().Length > 0;
        }
        
        private bool MethodHasEqualParameterTypesTo(IEnumerable<Type> comparedTypes)
        {
            IEnumerable<Type> myParameterTypes = SubscribingMethod.GetParameters().GetParameterTypes();
            return myParameterTypes.ContentEqual(comparedTypes);
        }

        private void InvokeMethod(object?[]? parameters)
        {
            SubscribingMethod.Invoke(Subscriber, parameters);
        }
    }

    internal class MethodSubscriptionComparer : IEqualityComparer<MethodSubscription>
    {
        public bool Equals(MethodSubscription x, MethodSubscription y)
        {
            return x.SubscribingMethod == y.SubscribingMethod && x.Subscriber == y.Subscriber;
        }

        public int GetHashCode(MethodSubscription obj)
        {
            return obj.GetHashCode();
        }
    }
}
