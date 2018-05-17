using Api.Modules.EventBus;

namespace Api
{
    public class SessionLocationChanged : EventMessage
    {
        public string SessionId { get; set; }
        public string NewSessionLocation { get; set; }
    }
}