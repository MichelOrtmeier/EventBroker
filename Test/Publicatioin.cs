using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBroker.EventAttributes;

namespace Test
{
    internal class Publication
    {
        [EventPublication("test")]
        public event EventHandler e;

        public void Test()
        {
            e(this, new EventArgs());
            Console.WriteLine("invoked");
        }

        public Publication()
        {
            CentralBroker.broker.Register(this);
        }
    }
}
