using Api.Modules.EventBus;
using Api.Modules.EventStore;

namespace Api.Common
{
    public class EventHandlerContext
    {
        public static EventHandlerContext Current;

        public IEventStore EventStore { get; set; }
        public IEventBus EventBus { get; set; }
    }
}