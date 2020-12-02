using Grpc.Core;
using PubSubServiceApi;
using System;

namespace SubscriberConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var subscriber = new SubscriberClient(() => new Channel("localhost:50051", ChannelCredentials.Insecure));
            subscriber.Subscribe(Guid.Empty.ToString("N"));

            subscriber.OnEventReceived += Subscriber_OnEventReceived;            

            Console.WriteLine("Press any key to exit subscriber console...");
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
