using PubSubServiceApi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PublisherConsoleClient
{
    public class AutoEventPublisherDemo : IDisposable
    {
        private bool disposedValue;
        private PubSubImpl _service;
        private CancellationTokenSource _cancellationTokenSource;

        public void PublishEvents()
        {
            _service = new PubSubImpl();
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _service.Publish(new Event()
                    {
                        Payload = $"And event for '{Guid.NewGuid()}' {Guid.NewGuid():N}",
                        PublisherId = Guid.Empty.ToString("N")
                    });

                    await Task.Delay(1000);
                }
            }, _cancellationTokenSource.Token);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _cancellationTokenSource?.Cancel();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AutoEventPublisherDemo()
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
