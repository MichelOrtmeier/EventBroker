using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBroker.EventAttributes;

namespace EventBroker.EventAttributes
{
    [AttributeUsage(AttributeTargets.Event, AllowMultiple = true)]
    public class EventPublicationAttribute : EventAttribute
    {
        public EventPublicationAttribute(string topic) : base(topic)
        {
        }
    }
}
