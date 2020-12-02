using System;
using System.Threading;
using System.Threading.Tasks;
using static PubSubServiceApi.PubSub;

namespace PubSubServiceApi
{
    public class EventSubscriber : Client
    {
        private static PubSubClient _pubSubClient;
        private Subscription _subscription;

        public EventSubscriber(PubSubClient pubSubClient) =>
            _pubSubClient = pubSubClient;

        public new event EventHandler<Event> OnEventReceived
        {
            add { base.OnEventReceived += value; }
            remove { base.OnEventReceived -= value; }
        }

        public override Task Publish(Event e)
        {
            return Task.Run(() => _pubSubClient.Publish(e));
        }

        public override async Task Subscribe(string subscriptionId)
        {
            _subscription = new Subscription() { Id = subscriptionId };
            Console.WriteLine($">> SubscriptionId : { subscriptionId}");
            using (var call = _pubSubClient.Subscribe(_subscription))
            {
                CancellationTokenSource = new CancellationTokenSource();
                await Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext(CancellationTokenSource.Token))
                    {
                        ReceiveEvent(call.ResponseStream.Current);
                    }
                }, CancellationTokenSource.Token);
            }
        }

        public override void Unsubscribe()
        {
            CancellationTokenSource?.Cancel();
            _pubSubClient?.Unsubscribe(_subscription);
        }
    }
}