using Grpc.Core;
using PubSubServiceApi;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PubSubServiceApi.Services;

namespace PubSubServer
{
    public class AutoEventPublisherDemo : Subscriber
    {
        private Server _server;

        public int Port { get; set; } = 50051;

        public void OpenServerAndPublishEvents()
        {
            var service = new PubSubImpl();
            _server = new Server
            {
                Services = { PubSub.BindService(service) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            _server.Start();

            CancellationTokenSource = new CancellationTokenSource();
            var randomGenerator = new Random(1000);
            //TODO split autopublisher into separate class
            Task.Run(async () =>
            {
                while (!CancellationTokenSource.IsCancellationRequested)
                {
                    if (service.SubscriberWritersMap.Count > 0)
                    {
                        var indexedKeys = service.SubscriberWritersMap.Select((kvp, idx) =>
                            new { Idx = idx, kvp.Key });

                        var subscriptionIdx = randomGenerator.Next(service.SubscriberWritersMap.Count);
                        var randomSubscriptionId = indexedKeys.Single(x => x.Idx == subscriptionIdx).Key;

                        service.Publish(new SubscriptionEvent()
                        {
                            Event = new Event()
                            {
                                Value = $"And event for '{randomSubscriptionId}' {Guid.NewGuid():N}"
                            },
                            SubscriptionId = randomSubscriptionId
                        });
                    }

                    await Task.Delay(1000);
                }
            }, CancellationTokenSource.Token);
        }

        
        public override Task Subscribe(string subscriptionId)
        {
            return Task.Run(() => OpenServerAndPublishEvents());
        }

        public override void Unsubscribe() => CancelAutoPublishAndShutDownServer();

        private void CancelAutoPublishAndShutDownServer()
        {
            CancellationTokenSource?.Cancel();
            _server?.ShutdownAsync().Wait();
        }

    }
}
