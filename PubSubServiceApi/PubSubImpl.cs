using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace PubSubServiceApi
{

    public class PubSubImpl : PubSub.PubSubBase
    {
        private readonly BufferBlock<Event> _buffer = new BufferBlock<Event>();

        public Dictionary<string, IServerStreamWriter<Event>> SubscriberWritersMap { get; private set; }

        public PubSubImpl()
        {
            SubscriberWritersMap = new Dictionary<string, IServerStreamWriter<Event>>();
        }

        public override Task<Event> GetAnEvent(Empty request, ServerCallContext context) =>
            Task.FromResult(new Event { Payload = DateTime.Now.ToLongTimeString() });

        public override Task<Empty> Publish(Event request, ServerCallContext context)
        {
            return Task.Run(() =>
            {                
                _buffer.Post(request);
                return new Empty();
            });
        }

        public override async Task Subscribe(Subscription request, IServerStreamWriter<Event> responseStream, ServerCallContext context)
        {
            SubscriberWritersMap[request.Id] = responseStream;

            while (SubscriberWritersMap.Count > 0)
            {
                var subscriptionEvent = await _buffer.ReceiveAsync();
                if (SubscriberWritersMap.ContainsKey(subscriptionEvent.PublisherId))
                {
                    await SubscriberWritersMap[subscriptionEvent.PublisherId].WriteAsync(subscriptionEvent);
                }
            }
        }

        public override Task<Unsubscription> Unsubscribe(Subscription request, ServerCallContext context)
        {
            SubscriberWritersMap.Remove(request.Id);
            return Task.FromResult(new Unsubscription() { Id = request.Id });
        }

    }
}
