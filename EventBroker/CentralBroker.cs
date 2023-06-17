using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker
{
    public static class CentralBroker
    {
        public static Broker Broker { get; } = new Broker();
    }
}
