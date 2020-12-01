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

        public EventSubscriber(PubSubClient pubSubClient) => 
            _pubSubClient = pubSubClient;

        public override async Task Subscribe(string subscriptionId)
        {
            _subscription = new Subscription() { Id = subscriptionId };
            Console.WriteLine($">> SubscriptionId : { subscriptionId}");
            using(var call = _pubSubClient.Subscribe(_subscription))
            {
                var cancellation = new CancellationTokenSource();
                await Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext(cancellation.Token))
                    {
                        SendEvent(call.ResponseStream.Current);                        
                    }
                });
            }
        }

        public override void Unsubscribe()
        {
            _pubSubClient.Unsubscribe(_subscription);
        }
    }

    
}
