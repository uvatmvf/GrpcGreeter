using PubSubServiceApi;
using System;
using System.Threading;
using System.Threading.Tasks;
using static PubSubServiceApi.PubSub;

namespace PubSubConsoleClient
{
    public class Subscriber
    {
        private static PubSubClient _pubSubClient;
        private Subscription _subscription;

        public Subscriber(PubSubClient pubSubClient) => 
            _pubSubClient = pubSubClient;

        public async Task Subscribe(string subscriptionId)
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
                        Console.WriteLine($"Event received: {call.ResponseStream.Current}");
                    }
                });
            }
        }

        public void Unsubscribe()
        {
            _pubSubClient.Unsubscribe(_subscription);
        }
    }
}
