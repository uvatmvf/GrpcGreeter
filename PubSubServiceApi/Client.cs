using System;
using System.Threading;
using System.Threading.Tasks;

namespace PubSubServiceApi
{
    public abstract class Client
    {
        protected CancellationTokenSource CancellationTokenSource { get; set; }
            = new CancellationTokenSource();

        public event EventHandler<Event> OnEventReceived;
        public void ReceiveEvent(Event e) => OnEventReceived?.Invoke(this, e);
        public abstract Task Publish(Event e);
        public abstract Task Subscribe(string subscriptionId);
        public abstract void Unsubscribe();
    }
}
