﻿using System;
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
            if (MethodHasEqualParameterTypesTo(GetTypesFromObjects(parameters)))
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
            IEnumerable<Type> myParameterTypes = GetTypesFromParameters(SubscribingMethod.GetParameters());
            return TypesAreEqual(myParameterTypes, comparedTypes);
        }

        private void InvokeMethod(object[] parameters)
        {
            SubscribingMethod.Invoke(Subscriber, parameters);
        }

        private static IEnumerable<Type> GetTypesFromObjects(IEnumerable<object> objects)
        {
            return objects.Select((obj) => obj.GetType());
        }

        private static IEnumerable<Type> GetTypesFromParameters(IEnumerable<ParameterInfo> parameters)
        {
            return parameters.Select((parameter) => parameter.ParameterType);
        }

        private static bool TypesAreEqual(IEnumerable<Type> types, IEnumerable<Type> comparedTypes)
        {
            IEnumerable<Type> orderedTypes = OrderEnumerableByHashCode(types);
            IEnumerable<Type> comparedOrderedTypes = OrderEnumerableByHashCode(comparedTypes);
            return orderedTypes.SequenceEqual(comparedOrderedTypes);
        }
        
        private static IEnumerable<T> OrderEnumerableByHashCode<T>(IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy((obj) => obj?.GetHashCode() ?? 0);
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
