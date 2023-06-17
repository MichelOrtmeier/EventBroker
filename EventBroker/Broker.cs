using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker.EventBroker
{
    delegate void EditTopicMember(object myObject, MemberInfo member, EventAttribute attribute);
    public class Broker
    {
        Dictionary<string, Topic> topics = new Dictionary<string, Topic>();

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

        private Topic GetCreateTopic(EventAttribute attribute)
        {
            Topic topic;
            if (!topics.ContainsKey(attribute.TopicName))
            {
                topic = new Topic(attribute.TopicName);
                topics.Add(attribute.TopicName, topic);
            }
            else
            {
                topic = topics[attribute.TopicName];
            }
            return topic;
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
            Topic topic = GetCreateTopic(attribute);
            switch (member)
            {
                case MethodInfo myMethod:
                    topic.AddMethod(myMethod, myObject);
                    break;
                case EventInfo myEvent:
                    //MethodInfo method = GetType().GetMethod(nameof(OnEventIsPublished) ,BindingFlags.NonPublic | BindingFlags.Instance);
                    //Delegate handler = Delegate.CreateDelegate(myEvent.EventHandlerType, myObject, method);
                    EventHandler handler = (sender, args) => OnEventIsPublished(sender, args, topic.TopicName);
                    myEvent.AddEventHandler(myObject, handler);
                    break;
            }
        }

        private void RemoveTopicMember(object myObject, MemberInfo member, EventAttribute attribute)
        {
            Topic topic;
            if (member is MethodInfo method && topics.TryGetValue(attribute.TopicName, out topic))
            {
                topic.RemoveSubscriber(myObject);
                if (topic.GetSubscriberLength() == 0)
                    topics.Remove(attribute.TopicName);
            }
        }
        #endregion
        #endregion

        #region EventRaisedMethods
        private void OnEventIsPublished(object sender, EventArgs args, string topicName)
        {
            Topic topic;
            if (topics.TryGetValue(topicName, out topic))
            {
                topic.CallMethods(sender, args);
            }
        }
        #endregion
    }
}
