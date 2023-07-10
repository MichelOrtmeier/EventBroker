using EventBroker.Extensions;
using System.Reflection;

namespace EventBroker
{
    public class MethodSubscription
    { 
        public object Subscriber { get; }
        public MethodInfo SubscribingMethod { get; }

        public MethodSubscription(MethodInfo subscribingMethod, object subscriber)
        {
            SubscribingMethod = subscribingMethod;
            Subscriber = subscriber;
        }

        public void InvokeMethodWithParameters(object[] parameters)
        {
            if (!MethodHasParameters())
            {
                InvokeMethod(null);
            }
            else if (MethodHasEqualParameterTypesTo(parameters))
            {
                InvokeMethod(parameters);
            }
            else
            {
                throw new ArgumentException($"The compared types of '{SubscribingMethod.Name}'" +
                                $"did neither match the given parameters nor had no required parameters");
            }
        }

        private bool MethodHasEqualParameterTypesTo(IEnumerable<object> passedObjects)
        {
            ParameterInfo[] requiredParameters = SubscribingMethod.GetParameters()
                                                                  .ToArray();
            Type[] requiredTypes = requiredParameters.GetParameterTypes()
                                                     .ToArray();
            Type[] passedTypes = passedObjects.GetObjectTypes()
                                              .ToArray();
            return requiredTypes.ContentEqual(passedTypes);
        }

        private bool MethodHasParameters()
        {
            return SubscribingMethod.GetParameters().Length > 0;
        }

        private void InvokeMethod(object?[]? parameters)
        {
            SubscribingMethod.Invoke(Subscriber, parameters);
        }
    }
}
