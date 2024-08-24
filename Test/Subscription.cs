using EventBroker.EventAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Subscription
    {
        [EventSubscription("test")]
        public void subscribe()
        {
            Console.WriteLine("subscribe");
        }

        [EventSubscription("test")]
        public void test(object obj, EventArgs e)
        {
            Console.WriteLine($"{obj.ToString()} {e.ToString()}");
        }

        public Subscription()
        {
            CentralBroker.broker.Register(this);
        }
    }
}
