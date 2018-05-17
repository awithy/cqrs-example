using Api.Modules.EventBus;

namespace Api.Events
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SessionLocationChanged : EventMessage
    {
        public string SessionId { get; set; }
        public string NewSessionLocation { get; set; }
    }
}