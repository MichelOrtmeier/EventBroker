using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EventSubscriptionAttribute : EventAttribute
    {
        public EventSubscriptionAttribute(string topic) : base(topic)
        {
        }
    }
}
