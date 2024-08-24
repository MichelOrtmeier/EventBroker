namespace EventBroker.MethodInstances
{
    public static class MethodInstanceListExtensions
    {
        public static void AddWithoutDuplicating(this List<MethodInstance> methods, MethodInstance newSubscriber)
        {
            if (!methods.Contains(newSubscriber, new MethodInstanceComparer()))
            {
                methods.Add(newSubscriber);
            }
        }
    }
}
