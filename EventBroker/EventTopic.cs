using System.Diagnostics;
using System.Reflection;
using EventBroker.MethodInstances;

namespace EventBroker
{ 
    public class EventTopic
    {
        public string TopicName { get; }

        private List<MethodInstance> subscribingMethods;//should be inherited

        public EventTopic(string topicName)
        {
            TopicName = topicName;
            subscribingMethods = new List<MethodInstance>();
        }

        public void AddSubscription(MethodInfo method, object subscriber)
        {
            MethodInstance newSubscribingMethod = MethodInstanceFactory.CreateMethodInstance(subscriber, method);
            if (!subscribingMethods.Contains(newSubscribingMethod, new MethodInstanceComparer()))
            {
                subscribingMethods.Add(newSubscribingMethod);
            }
        }

        public void RemoveAllSubscriptionsOf(object subscriber)
        {
            MethodInstance[] matches = subscribingMethods.Where((method) => method.Instance.Equals(subscriber)).ToArray();
            for (int i = matches.Length - 1; i >= 0; i--)
            {
                subscribingMethods.Remove(matches.ElementAt(i));
            }
        }

        public int CountSubscriptions()
        {
            return subscribingMethods.Count;
        }

        public void Fire(object sender, EventArgs args)
        {
            dynamic dynamicallyCastedArgs = args;
            object[] parameters = new object[] {sender, dynamicallyCastedArgs};
            for (int i = subscribingMethods.Count - 1; i >= 0; i--)
            {
                InvokeMethod(parameters, subscribingMethods[i]);
            }
        }

        private void InvokeMethod(object[] parameters, MethodInstance method)
        {
            try
            {
                method.Invoke(parameters);
            }
            catch (Exception ex) when (IsExceptionCausedByMethodInvocation(ex))
            {
                CatchFailedMethodInvocation(method, ex);
            }
        }

        private static bool IsExceptionCausedByMethodInvocation(Exception ex)
        {
            return ex is TargetException ||
                   ex is ArgumentException ||
                   ex is TargetInvocationException ||
                   ex is TargetParameterCountException ||
                   ex is MethodAccessException ||
                   ex is InvalidOperationException ||
                   ex is NotSupportedException;
        }

        private void CatchFailedMethodInvocation(MethodInstance method, Exception ex)
        {
            Debug.Write("While calling a subscribing method, something went wrong: ");
            Debug.WriteLine(ex.ToString());
            subscribingMethods.Remove(method);
        }
    }
}
