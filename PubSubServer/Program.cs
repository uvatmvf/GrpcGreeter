using PubSubServiceApi;
using PubSubServiceApi.Services;
using System;

namespace PubSubServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            var autoPublisher = new AutoEventPublisherDemo();

            autoPublisher.OpenServerAndPublishEvents();

            Console.WriteLine("Greeter server listening on port " + ServerService<PubSubImpl>.Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            autoPublisher.Unsubscribe();
        }
    }
}
