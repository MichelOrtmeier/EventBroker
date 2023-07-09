namespace EventBroker.EventAttributes
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
