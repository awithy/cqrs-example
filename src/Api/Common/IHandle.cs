using System.Threading.Tasks;
using Api.Modules.EventBus;

namespace Api.Common
{
    public interface IHandle<TEventMessage> where TEventMessage : EventMessage
    {
        Task Handle(TEventMessage @event);
    }
}