using EventBroker.EventAttributes;
using EventBroker.MethodInstances;
using System.Reflection;

namespace EventBroker
{
    public class Broker
    {
        Dictionary<string, List<MethodInstance>> topics = new Dictionary<string, List<MethodInstance>>();

        public void Register(object instance)
        {
            RegisterInstanceMethods(instance);
            RegisterInstanceEvents(instance);
        }

        private void RegisterInstanceMethods(object instance)
        {
            MethodInstance[] attributedMethods = instance.GetMethodInstancesWithAttribute<EventSubscriptionAttribute>();
            foreach (MethodInstance method in attributedMethods)
            {
                RegisterAttributedMethod(method);
            }
        }

        private void RegisterAttributedMethod(MethodInstance method)
        {
            EventSubscriptionAttribute[] attributes = method.Method.GetCustomAttributes<EventSubscriptionAttribute>()
                                                        .ToArray();
            foreach (EventSubscriptionAttribute attribute in attributes)
            {
                AddTopicByName(attribute.TopicName);
                topics[attribute.TopicName].AddWithoutDuplicating(method);
            }
        }

        private void AddTopicByName(string topicName)
        {
            if (!topics.ContainsKey(topicName))
                topics.Add(topicName, new List<MethodInstance>());
        }

        private void RegisterInstanceEvents(object instance)
        {
            EventInfo[] attributedEvents = instance.GetEventsWithAttribute<EventPublicationAttribute>();
            foreach (EventInfo myEvent in attributedEvents)
            {
                RegisterInstanceEvent(instance, myEvent);
            }
        }

        private void RegisterInstanceEvent(object instance, EventInfo myEvent)
        {
            string topicName = myEvent.GetCustomAttribute<EventPublicationAttribute>().TopicName;//Attribute müssen zweimal gesucht werden
            AddTopicByName(topicName);
            EventHandler handler = (sender, args) => new TopicFirer(topics[topicName], sender, args).Fire();
            myEvent.AddEventHandler(instance, handler);
        }

        public void UnregisterSubscriptionsOfInstance(object instance)
        {
            MethodInstance[] methods = instance.GetMethodInstancesWithAttribute<EventSubscriptionAttribute>();
            foreach (MethodInstance method in methods)
            {
                UnregisterSubscriptionsOfMethodInstance(method);
            }
        }

        private void UnregisterSubscriptionsOfMethodInstance(MethodInstance subscriber)
        {
            EventSubscriptionAttribute[] attributes = subscriber.Method.GetCustomAttributes<EventSubscriptionAttribute>().ToArray();
            foreach (EventSubscriptionAttribute attribute in attributes)
            {
                UnregisterSubscriptionOfMethodInstance(subscriber, attribute);
            }
        }

        private void UnregisterSubscriptionOfMethodInstance(MethodInstance method, EventSubscriptionAttribute attribute)
        {
            List<MethodInstance> topic;
            if (topics.TryGetValue(attribute.TopicName, out topic))
            {
                topic.Remove(method);
            }
        }
    }
}