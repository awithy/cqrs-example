using System.Threading.Tasks;

namespace Api.Common
{
    public interface IHandle<EventMessage>
    {
        Task Handle(EventMessage @event);
    }
}