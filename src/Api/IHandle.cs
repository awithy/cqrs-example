using System.Threading.Tasks;

namespace Api
{
    public interface IHandle<EventMessage>
    {
        Task Handle(EventMessage @event);
    }
}