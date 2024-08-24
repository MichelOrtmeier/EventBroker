namespace EventBroker.MethodInstances
{
    public class MethodInstanceComparer : IEqualityComparer<MethodInstance>
    {
        public bool Equals(MethodInstance x, MethodInstance y)
        {
            return x.Method == y.Method && x.Instance == y.Instance;
        }

        public int GetHashCode(MethodInstance obj)
        {
            return obj.GetHashCode();
        }
    }
}
