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
