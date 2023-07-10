using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker
{
    public class MethodSubscriptionComparer : IEqualityComparer<MethodSubscription>
    {
        public bool Equals(MethodSubscription x, MethodSubscription y)
        {
            return x.SubscribingMethod == y.SubscribingMethod && x.Subscriber == y.Subscriber;
        }

        public int GetHashCode(MethodSubscription obj)
        {
            return obj.GetHashCode();
        }
    }
}
