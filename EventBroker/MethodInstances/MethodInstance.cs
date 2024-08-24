using System.Reflection;

namespace EventBroker.MethodInstances
{
    public abstract class MethodInstance
    {
        public object Instance { get; set; }
        public MethodInfo Method { get; set; }

        protected MethodInstance(object instance, MethodInfo method)
        {
            Instance = instance;
            Method = method;
        }

        public abstract void Invoke(object[] parameters);
    }
}
