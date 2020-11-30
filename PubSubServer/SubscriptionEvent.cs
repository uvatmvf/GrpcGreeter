using PubSubService;

namespace PubSubServiceApi
{
    public class SubscriptionEvent
    {
        public Event EventArgs { get; set; }
        public string SubscriptionId { get; set; }
    }
}
