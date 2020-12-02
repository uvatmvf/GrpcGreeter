using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PubSubServiceApi
{
    public class SubscriberClient : Subscriber
    {
        private const int MillisecondsDelay = 2000;
        //TODO refactor common logic out of inheritance.
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
            CancellationTokenSource = new CancellationTokenSource();
            var loopTask = Task.Run(async () =>
            {
                while (!CancellationTokenSource.IsCancellationRequested)
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
            CancellationTokenSource?.Cancel();
            _subscriber?.Unsubscribe();
        }

    }
}
