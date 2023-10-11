using System.Reflection;
using EventBroker.EventAttributes;
using EventBroker.Extensions;

namespace EventBroker
{
    delegate void EditTopicMember(object myObject, MemberInfo member, EventAttribute attribute);
    public class Broker
    {
        Dictionary<string, EventTopic> topicNameTopicPairs = new Dictionary<string, EventTopic>();

        #region RegisterMethods
        public void Register(object myObject)
        {
            Type myObjectType = myObject.GetType();
            EditTopics(myObjectType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly), myObject, AddTopicMember);
            EditTopics(myObjectType.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly), myObject, AddTopicMember);
        }

        public void Unregister(object myObject)
        {
            Type myObjectType = myObject.GetType();
            EditTopics(myObjectType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly), myObject, RemoveTopicMember);
        }
        #endregion

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

        private static IEnumerable<MemberInfo> GetMembersWithAttribute<TAttribute>(MemberInfo[] members) where TAttribute : Attribute
        {
            var filteredMembers = from member in members
                                  where member.GetCustomAttributes<TAttribute>(true).Count() > 0
                                  select member;
            return filteredMembers;
        }

        #region EditingOptions
        private void AddTopicMember(object myObject, MemberInfo member, EventAttribute attribute)
        {
            EventTopic topic = topicNameTopicPairs.GetOrCreateTopic(attribute.TopicName);
            switch (member)
            {
                case MethodInfo myMethod:
                    topic.AddSubscription(myMethod, myObject);
                    break;
                case EventInfo myEvent:
                    //MethodInfo method = GetType().GetMethod(nameof(OnEventIsPublished) ,BindingFlags.NonPublic | BindingFlags.Instance);
                    //Delegate handler = Delegate.CreateDelegate(myEvent.EventHandlerType, myObject, method);
                    EventHandler handler = (sender, args) => OnEventIsFired(sender, args, topic.TopicName);
                    myEvent.AddEventHandler(myObject, handler);
                    break;
            }
        }

        private void RemoveTopicMember(object myObject, MemberInfo member, EventAttribute attribute)
        {
            EventTopic topic;
            if (member is MethodInfo method && topicNameTopicPairs.TryGetValue(attribute.TopicName, out topic))
            {
                topic.RemoveAllSubscriptionsOf(myObject);
                if (topic.CountSubscriptions() == 0)
                    topicNameTopicPairs.Remove(attribute.TopicName);
            }
        }
        #endregion
        #endregion

        #region EventRaisedMethods
        private void OnEventIsFired(object sender, EventArgs args, string topicName)
        {
            EventTopic topic;
            if (topicNameTopicPairs.TryGetValue(topicName, out topic))
            {
                topic.Fire(sender, args);
            }
        }
        #endregion
    }
}
