using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PubSubServiceApi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PubSubConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = new Channel("localhost:50051", ChannelCredentials.Insecure );
            var client = new PubSub.PubSubClient(channel);
            var subscriber = new Subscriber(client);

            var cancellation = new CancellationTokenSource();
            var loopTask = Task.Run(async () =>
            {
                while (!cancellation.IsCancellationRequested)
                {
                    var reply = client.GetAnEvent(new Empty());
                    Console.WriteLine($"GetAnEvent : {reply}");
                    await Task.Delay(2000);
                }
            }).ConfigureAwait(false).GetAwaiter();

            Task.Run(async () =>
            {
                await subscriber.Subscribe(Guid.NewGuid().ToString("N"));
            }).ConfigureAwait(false).GetAwaiter();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
            cancellation.Cancel();            
            subscriber.Unsubscribe();            
            Console.WriteLine("Unsubscribed");
        }
    }
}
