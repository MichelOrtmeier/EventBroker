using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
