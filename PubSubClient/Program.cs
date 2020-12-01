using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PubSubServiceApi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscriberConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var subscriber = new SubscriberClient(() => new Channel("localhost:50051", ChannelCredentials.Insecure));
            subscriber.Subscribe(Guid.NewGuid().ToString("N"));

            subscriber.OnEventReceived += Subscriber_OnEventReceived;            

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
            subscriber.Unsubscribe();
            Console.WriteLine("Unsubscribed");
        }

        private static void Subscriber_OnEventReceived(object sender, Event e)
        {
            Console.WriteLine($"Event received: {e}");
        }
    }
}
