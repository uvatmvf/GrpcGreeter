using Grpc.Core;
using PubSubServiceApi;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PubSubServiceApi.Services;

namespace PubSubServer
{
    public class AutoEventPublisherDemo : Subscriber, IDisposable
    {
        private ServerService<PubSubImpl> _server;
        private bool disposedValue;

        public void OpenServerAndPublishEvents()
        {
            _server = new ServerService<PubSubImpl>();            
            _server.Start();
            
            CancellationTokenSource = new CancellationTokenSource();

            var randomGenerator = new Random(1000);
            //TODO split autopublisher into separate class
            Task.Run(async () =>
            {
                while (!CancellationTokenSource.IsCancellationRequested)
                {
                    if (_server.Service.SubscriberWritersMap.Count > 0)
                    {
                        var indexedKeys = _server.Service.SubscriberWritersMap.Select((kvp, idx) =>
                            new { Idx = idx, kvp.Key });

                        var subscriptionIdx = randomGenerator.Next(_server.Service.SubscriberWritersMap.Count);
                        var randomSubscriptionId = indexedKeys.Single(x => x.Idx == subscriptionIdx).Key;

                        _server.Service.Publish(new SubscriptionEvent()
                        {
                            Event = new Event()
                            {
                                Value = $"And event for '{randomSubscriptionId}' {Guid.NewGuid():N}"
                            },
                            SubscriptionId = randomSubscriptionId
                        });
                    }

                    await Task.Delay(1000);
                }
            }, CancellationTokenSource.Token);
        }

        
        public override Task Subscribe(string subscriptionId)
        {
            return Task.Run(() => OpenServerAndPublishEvents());            
        }

        public override void Unsubscribe() => CancelAutoPublishAndShutDownServer();

        private void CancelAutoPublishAndShutDownServer()
        {
            CancellationTokenSource?.Cancel();
            _server?.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _server?.Dispose();
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
