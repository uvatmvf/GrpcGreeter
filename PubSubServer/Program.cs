using Grpc.Core;
using PubSubServiceApi;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PubSubServer
{
    class Program
    {
        const int Port = 50051;

        public static void Main(string[] args)
        {
            var service = new PubSubServiceApi.Services.PubSubImpl();
            Server server = new Server
            {
                Services = { PubSub.BindService(service) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            var cancellationToken = new CancellationTokenSource();
            var randomGenerator = new Random(1000);

            Task.Run(async () =>
            {
                while(!cancellationToken.IsCancellationRequested)
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
            }, cancellationToken.Token);

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            cancellationToken.Cancel();
            server.ShutdownAsync().Wait();
        }
    }
}
