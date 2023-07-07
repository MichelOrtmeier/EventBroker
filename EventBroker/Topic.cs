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

        public void InvokeSubscribingMethods(object sender, EventArgs args)
        {
            dynamic e = args;
            for (int i = subscribingMethods.Count - 1; i >= 0; i--)
            {
                MethodSubscription method = subscribingMethods[i];
                try
                {
                    ParameterInfo[] parameters = method.MyMethodInfo.GetParameters();
                    if (parameters.Length == 2
                        && parameters.Count((param) => param.ParameterType == typeof(object)) == 1
                        && parameters.Count((param) => param.ParameterType == args.GetType()) == 1)
                    {
                        method.MyMethodInfo.Invoke(method.Subscriber, new object[] { sender, e });
                    }
                    else if (method.MyMethodInfo.GetParameters().Length == 0)
                    {
                        method.MyMethodInfo.Invoke(method.Subscriber, null);
                    }
                    else
                    {
                        Debug.Write("While calling a subscribing method, something went wrong: " +
                            "\n The method did not match the correct parameter specifications.");
                        subscribingMethods.Remove(method);
                    }
                }
                catch (Exception ex) when (ex is TargetException ||
                                            ex is ArgumentException ||
                                            ex is TargetInvocationException ||
                                            ex is TargetParameterCountException ||
                                            ex is MethodAccessException ||
                                            ex is InvalidOperationException ||
                                            ex is NotSupportedException)
                {
                    Debug.Write("While calling a subscribing method, something went wrong: ");
                    Debug.WriteLine(ex.ToString());
                    subscribingMethods.Remove(method);
                }
            }
        }

        public int CountSubscribingMethods()
        {
            return subscribingMethods.Count;
        }
    }
}
