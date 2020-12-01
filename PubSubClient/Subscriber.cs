using PubSubServiceApi;
using System;
using System.Threading.Tasks;

namespace SubscriberConsoleClient
{
    public abstract class Subscriber
    {
        public event EventHandler<Event> OnEventReceived;
        public void SendEvent(Event e) => OnEventReceived?.Invoke(this, e);
        public abstract Task Subscribe(string subscriptionId);
        public abstract void Unsubscribe();
    }
}
