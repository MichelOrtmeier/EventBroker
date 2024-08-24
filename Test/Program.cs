using EventBroker;
using EventBroker.EventAttributes;
using Test;

Publication publication = new Publication();
Publication second = new Publication();
var first = new Subscription();
new Subscription();
new Subscription();
new Subscription();
publication.Test();
publication.Test();
publication.Test();
publication.Test();
second.Test();
CentralBroker.broker.Unregister(first);
second.Test();