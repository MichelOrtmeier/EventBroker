using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker.EventBroker
{
    public abstract class EventAttribute : Attribute
    {
        public string TopicName { get; }

        public EventAttribute(string topic)
        {
            TopicName = topic;
        }
    }
}
