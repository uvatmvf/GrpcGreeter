using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task SayHelloStreamingFromServer(HelloRequest request,
            IServerStreamWriter<HelloReply> responseStream, 
            ServerCallContext context)
        {
            for(var i = 0; i < 5 && !context.CancellationToken.IsCancellationRequested; i++)
            {
                await responseStream.WriteAsync(new HelloReply { Message = $"Hello {request.Name}" });
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }
    }
}
