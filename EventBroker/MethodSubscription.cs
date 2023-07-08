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
        public MethodInfo MyMethodInfo { get; }
        public object Subscriber { get; }

        public MethodSubscription(MethodInfo myMethodInfo, object subscriber)
        {
            MyMethodInfo = myMethodInfo;
            Subscriber = subscriber;
        }

        public void InvokeMethodWithParameters(object[] parameters)
        {
            if (MethodHasEqualParameterTypes(GetTypesFromObjects(parameters)))
            {
                InvokeMethod(parameters);
            }
            else if (!MethodHasParameters())
            {
                InvokeMethod(null);
            }
            else
            {
                ThrowUnmatchingParametersException();
            }
        }

        private void ThrowUnmatchingParametersException()
        {
            throw new ArgumentException($"The comparedTypes of '{MyMethodInfo.Name}'" +
                                $"did neither match the given EventArgs subtype and the object sender nor " +
                                $"no comparedTypes.");
        }

        private bool MethodHasParameters()
        {
            return MyMethodInfo.GetParameters().Length > 0;
        }

        private bool MethodHasEqualParameterTypes(Type[] comparedTypes)
        {
            Type[] myParameterTypes = GetTypesFromParameters(MyMethodInfo.GetParameters());
            return TypesAreEqual(myParameterTypes, comparedTypes);
        }

        private Type[] GetTypesFromObjects(object[] objects)
        {
            return objects.Select((obj) => obj.GetType()).ToArray();
        }

        private Type[] GetTypesFromParameters(ParameterInfo[] parameters)
        {
            return parameters.Select((parameter) => parameter.ParameterType).ToArray();
        }

        private bool TypesAreEqual(Type[] types, Type[] comparedTypes)
        {
            return OrderEnumerableByHashCode(types).SequenceEqual(OrderEnumerableByHashCode(comparedTypes));
        }
        
        private IEnumerable<T> OrderEnumerableByHashCode<T>(IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy((obj) => obj?.GetHashCode() ?? 0);
        }

        private void InvokeMethod(object?[]? parameters)
        {
            MyMethodInfo.Invoke(Subscriber, parameters);
        }
    }

    internal class MethodSubscriptionComparer : IEqualityComparer<MethodSubscription>
    {
        public bool Equals(MethodSubscription x, MethodSubscription y)
        {
            return x.MyMethodInfo == y.MyMethodInfo && x.Subscriber == y.Subscriber;
        }

        public int GetHashCode(MethodSubscription obj)
        {
            return obj.GetHashCode();
        }
    }
}
