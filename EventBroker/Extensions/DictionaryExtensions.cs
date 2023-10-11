using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker.Extensions
{
    public static class DictionaryExtensions
    {
        public static EventTopic GetOrCreateTopic(this Dictionary<string, EventTopic> topicNameTopicPairs, string topicName)
        {
            EventTopic topic;
            if (!topicNameTopicPairs.ContainsKey(topicName))
            {
                topic = new EventTopic(topicName);
                topicNameTopicPairs.Add(topicName, topic);
            }
            else
            {
                topic = topicNameTopicPairs[topicName];
            }
            return topic;
        }
    }
}
