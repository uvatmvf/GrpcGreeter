using PubSubServiceApi;

namespace PubSubServer
{
    public class SubscriptionEvent
    {
        public Event Event { get; set; }
        public string SubscriptionId { get; set; }
    }
}
