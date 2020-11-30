using Grpc.Net.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcGreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);

            // Unary sample
            var reply = await client.SayHelloAsync(new HelloRequest() { Name = "GreeterClient" });
            Console.WriteLine($"Greeting: {reply.Message}");

            // server stream example
            var cancellationToken = new CancellationTokenSource();
            var replyStream = client.SayHelloStreamingFromServer(new HelloRequest() { Name = "StreamGreeterClient" },
                cancellationToken: cancellationToken.Token);
            await replyStream.ResponseStream.MoveNext(cancellationToken.Token);
            Console.WriteLine($"Stream greeting: {replyStream.ResponseStream.Current.Message}");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
