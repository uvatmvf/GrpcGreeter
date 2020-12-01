using PubSubServiceApi;
using System;
using System.Threading;
using System.Threading.Tasks;
using static PubSubServiceApi.PubSub;

namespace SubscriberConsoleClient
{
    public class EventSubscriber : Subscriber
    {
        private static PubSubClient _pubSubClient;
        private Subscription _subscription;
        private CancellationTokenSource _cancellationTokenSource;

        public EventSubscriber(PubSubClient pubSubClient) => 
            _pubSubClient = pubSubClient;

        public new event EventHandler<Event> OnEventReceived
        {
            add { base.OnEventReceived += value; }
            remove { base.OnEventReceived -= value; }
        }

        public override async Task Subscribe(string subscriptionId)
        {
            _subscription = new Subscription() { Id = subscriptionId };
            Console.WriteLine($">> SubscriptionId : { subscriptionId}");
            using(var call = _pubSubClient.Subscribe(_subscription))
            {
                _cancellationTokenSource = new CancellationTokenSource();
                await Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext(_cancellationTokenSource.Token))
                    {
                        ReceiveEvent(call.ResponseStream.Current);                        
                    }
                }, _cancellationTokenSource.Token);
            }
        }

        public override void Unsubscribe()
        {
            _cancellationTokenSource.Cancel();
            _pubSubClient.Unsubscribe(_subscription);
        }
    }    
}