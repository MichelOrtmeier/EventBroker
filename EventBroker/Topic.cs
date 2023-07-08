using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace EventBroker
{
    public class Topic
    {
        public string TopicName { get; }

        private List<MethodSubscription> subscribingMethods;

        public Topic(string topicName)
        {
            TopicName = topicName;
            subscribingMethods = new List<MethodSubscription>();
        }

        public void AddMethodSubscription(MethodInfo method, object subscriber)
        {
            MethodSubscription newSubscribingMethod = new MethodSubscription(method, subscriber);
            if (!subscribingMethods.Contains(newSubscribingMethod, new MethodSubscriptionComparer()))
            {
                subscribingMethods.Add(newSubscribingMethod);
            }
        }

        public void RemoveMethodSubscriptionsOfSubscriber(object subscriber)
        {
            MethodSubscription[] matches = subscribingMethods.Where((method) => method.Subscriber.Equals(subscriber)).ToArray();
            for (int i = matches.Length - 1; i >= 0; i--)
            {
                subscribingMethods.Remove(matches.ElementAt(i));
            }
        }

        public void InvokeSubscribingMethodsWithEventParameters(object sender, EventArgs args)
        {
            dynamic dynamicallyCastedArgs = args;
            object[] parameters = new object[] {sender, dynamicallyCastedArgs};
            for (int i = subscribingMethods.Count - 1; i >= 0; i--)
            {
                InvokeSubscribingMethod(parameters, subscribingMethods[i]);
            }
        }

        private void InvokeSubscribingMethod(object[] parameters, MethodSubscription method)
        {
            try
            {
                method.InvokeMethodWithParameters(parameters);
            }
            catch (Exception ex) when (ExceptionIsMethodInvocationException(ex))
            {
                CatchFailedMethodInvocation(method, ex);
            }
        }

        private void CatchFailedMethodInvocation(MethodSubscription method, Exception ex)
        {
            Debug.Write("While calling a subscribing method, something went wrong: ");
            Debug.WriteLine(ex.ToString());
            subscribingMethods.Remove(method);
        }

        private static bool ExceptionIsMethodInvocationException(Exception ex)
        {
            return ex is TargetException ||
                   ex is ArgumentException ||
                   ex is TargetInvocationException ||
                   ex is TargetParameterCountException ||
                   ex is MethodAccessException ||
                   ex is InvalidOperationException ||
                   ex is NotSupportedException;
        }
        public int CountSubscribingMethods()
        {
            return subscribingMethods.Count;
        }
    }
}
