using EventBroker.EventAttributes;
using EventBroker.MethodInstances;
using System.Reflection;

namespace EventBroker
{
    public class Broker
    {
        Dictionary<string, EventTopic> topics = new Dictionary<string, EventTopic>();

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

        #region EditTopics
        private void EditTopics(MemberInfo[] members, object myObject, EditTopicMember action)
        {
            var attributedMembers = GetMembersWithAttribute<EventAttribute>(members);

            foreach (MemberInfo member in attributedMembers)
            {
                foreach (EventAttribute attribute in member.GetCustomAttributes<EventAttribute>(true))
                {
                    action(myObject, member, attribute);
                }
            }
        }

        private EventTopic GetCreateTopic(EventAttribute attribute)
        {
            EventTopic topic;
            if (!topics.ContainsKey(attribute.TopicName))
            {
                topic = new EventTopic(attribute.TopicName);
                topics.Add(attribute.TopicName, topic);
            }
            else
            {
                topic = topics[attribute.TopicName];
            }
            return topic;
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
            EventTopic topic = GetCreateTopic(attribute);
            switch (member)
            {
                case MethodInfo myMethod:
                    topic.AddSubscription(myMethod, myObject);
                    break;
                case EventInfo myEvent:
                    //MethodInfo method = GetType().GetMethod(nameof(OnEventIsPublished) ,BindingFlags.NonPublic | BindingFlags.Instance);
                    //Delegate handler = Delegate.CreateDelegate(myEvent.EventHandlerType, myObject, method);
                    EventHandler handler = (sender, args) => OnEventIsPublished(sender, args, topic.TopicName);
                    myEvent.AddEventHandler(myObject, handler);
                    break;
            }
        }

        private void UnregisterSubscriptionsOfMethodInstance(MethodInstance subscriber)
        {
            EventTopic topic;
            if (member is MethodInfo method && topics.TryGetValue(attribute.TopicName, out topic))
            {
                topic.RemoveAllSubscriptionsOf(myObject);
                if (topic.CountSubscriptions() == 0)
                    topics.Remove(attribute.TopicName);
            }
        }

        #region EventRaisedMethods
        private void OnEventIsPublished(object sender, EventArgs args, string topicName)
        {
            EventTopic topic;
            if (topics.TryGetValue(topicName, out topic))
            {
                topic.Remove(method);
            }
        }
    }
}