using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker.EventBroker
{
    [AttributeUsage(AttributeTargets.Event, AllowMultiple = true)]
    public class EventPublicationAttribute : EventAttribute
    {
        public EventPublicationAttribute(string topic) : base(topic)
        {
        }
    }
}
