namespace EventBroker.EventAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EventSubscriptionAttribute : EventAttribute
    {
        public EventSubscriptionAttribute(string topic) : base(topic)
        {
        }
    }
}
