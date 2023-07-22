using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
