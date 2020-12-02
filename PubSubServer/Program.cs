using PubSubServiceApi;
using System;

namespace PubSub.ServerConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            var server = new ServerService<PubSubImpl>();
            server.Start();
            Console.WriteLine("PubSub server listening on port " + ServerService<PubSubImpl>.Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            server.Dispose();
        }
    }
}
