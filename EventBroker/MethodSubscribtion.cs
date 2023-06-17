using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker.EventBroker
{
    public class MethodSubscribtion
    {
        public MethodInfo MyMethodInfo { get; }
        public object Subscriber { get; }

        public MethodSubscribtion(MethodInfo myMethodInfo, object subscriber)
        {
            MyMethodInfo = myMethodInfo;
            Subscriber = subscriber;
        }

        public void CallMethod()
        {
            MyMethodInfo.Invoke(Subscriber, null);
        }
    }

    internal class MethodSubscribtionComparer : IEqualityComparer<MethodSubscribtion>
    {
        public bool Equals(MethodSubscribtion x, MethodSubscribtion y)
        {
            return x.MyMethodInfo == y.MyMethodInfo && x.Subscriber == y.Subscriber;
        }

        public int GetHashCode(MethodSubscribtion obj)
        {
            return obj.GetHashCode();
        }
    }
}
