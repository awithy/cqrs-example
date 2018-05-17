using Api.Modules.EventBus;

namespace Api.Events
{
    public class SessionCancelledEvent : EventMessage
    {
        public string SessionId { get; set; }
    }
}