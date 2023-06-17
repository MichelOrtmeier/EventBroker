using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace EventBroker.EventBroker
{
    public class Topic
    {
        public string TopicName { get; }

        private List<MethodSubscribtion> methods;

        public Topic(string topicName)
        {
            TopicName = topicName;
            methods = new List<MethodSubscribtion>();
        }

        public void AddMethod(MethodInfo method, object subscriber)
        {
            MethodSubscribtion newSubscription = new MethodSubscribtion(method, subscriber);
            if (!methods.Contains(newSubscription, new MethodSubscribtionComparer()))
            {
                methods.Add(newSubscription);
            }
        }

        public void RemoveSubscriber(object subscriber)
        {
            MethodSubscribtion[] matches = methods.Where((method) => method.Subscriber.Equals(subscriber)).ToArray();
            for (int i = matches.Count() - 1; i >= 0; i--)
            {
                methods.Remove(matches.ElementAt(i));
            }
        }

        public void CallMethods(object sender, EventArgs args)
        {
            dynamic e = args;
            for (int i = methods.Count() - 1; i >= 0; i--)
            {
                MethodSubscribtion method = methods[i];
                try
                {
                    ParameterInfo[] parameters = method.MyMethodInfo.GetParameters();
                    if (parameters.Count() == 2
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
                        methods.Remove(method);
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
                    methods.Remove(method);
                }
            }
        }

        public int GetSubscriberLength()
        {
            return methods.Count;
        }
    }
}
