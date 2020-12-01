using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PubSubServiceApi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscriberConsoleClient
{
    public class SubscriberClient : Subscriber
    {
        private const int MillisecondsDelay = 2000;
        private CancellationTokenSource _cancellationToken;
        private Subscriber _subscriber;
        private Func<Channel> _channelFactory;

        public SubscriberClient(Func<Channel> channelFactory) => 
            _channelFactory = channelFactory;

        public new event EventHandler<Event> OnEventReceived
        {
            add { _subscriber.OnEventReceived += value; }
            remove { _subscriber.OnEventReceived -= value; }
        }
        public Action<string> OnGetAnEvent { get; set; }

        public override Task Subscribe(string subscriptionId)
        {
            var channel = _channelFactory.Invoke();
            var client = new PubSub.PubSubClient(channel);
            _subscriber = new EventSubscriber(client);
            _cancellationToken = new CancellationTokenSource();
            var loopTask = Task.Run(async () =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    var reply = client.GetAnEvent(new Empty());
                    OnGetAnEvent?.Invoke($"GetAnEvent : {reply}");
                    await Task.Delay(MillisecondsDelay);
                }
            }).ConfigureAwait(false).GetAwaiter();

            return Task.Run(async () =>
            {
                await _subscriber.Subscribe(subscriptionId);
            });
        }

        public override void Unsubscribe()
        {
            _cancellationToken?.Cancel();
            _subscriber?.Unsubscribe();
        }

    }
}
