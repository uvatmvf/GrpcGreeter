using Grpc.Core;
using System;
using static PubSubServiceApi.PubSub;

namespace PubSubServiceApi
{
    public class ServerService<T> : IDisposable where T : PubSubBase, new()
    {
        private bool disposedValue;
        private Server _server;
        public static int Port { get; set; } = 50051;
        public T Service { get; private set; }

        public void Start()
        {
            Service = new T();
            _server = new Server
            {
                Services = { PubSub.BindService(Service) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            _server.Start();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _server?.KillAsync().Wait();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ServerService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
