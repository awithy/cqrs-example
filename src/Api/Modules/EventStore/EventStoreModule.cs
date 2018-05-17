using System.Threading.Tasks;

namespace Api.Modules.EventStore
{
    public class EventStoreModule : IInitializable
    {
        public Task Initialize()
        {
            return Task.CompletedTask;
        }
    }
}