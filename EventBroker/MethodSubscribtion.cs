using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker
{
    public class MethodSubscription
    {
        public MethodInfo MyMethodInfo { get; }
        public object Subscriber { get; }

        public MethodSubscription(MethodInfo myMethodInfo, object subscriber)
        {
            MyMethodInfo = myMethodInfo;
            Subscriber = subscriber;
        }

        public void CallMethod()
        {
            MyMethodInfo.Invoke(Subscriber, null);
        }
    }

    internal class MethodSubscriptionComparer : IEqualityComparer<MethodSubscription>
    {
        public bool Equals(MethodSubscription x, MethodSubscription y)
        {
            return x.MyMethodInfo == y.MyMethodInfo && x.Subscriber == y.Subscriber;
        }

        public int GetHashCode(MethodSubscription obj)
        {
            return obj.GetHashCode();
        }
    }
}
