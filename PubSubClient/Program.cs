using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using PubSubService;
using System;

namespace PubSubClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:50051", 
                new GrpcChannelOptions() { Credentials = ChannelCredentials.Insecure });
            //var client = new PubSubService.PubSub.PubSubClient(channel);
            //var reply = client.GetAnEvent(new Empty());
            //Console.WriteLine($"GetAnEvent: { reply }");
        }
    }
}
