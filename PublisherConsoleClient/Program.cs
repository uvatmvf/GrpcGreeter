using System;

namespace PublisherConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var demo = new ServerEventPublisherDemo();
            demo.PublishEvents();

            Console.WriteLine("Press any key to stop the auto event publisher...");
            Console.ReadKey();
            demo.Unsubscribe();
        }
    }
}
