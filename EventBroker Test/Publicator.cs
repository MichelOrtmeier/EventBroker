using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBroker.EventAttributes;

namespace EventBroker_Test
{
    internal class Publicator
    {
        [EventPublication]
        public event EventHandler e;
    }
}
