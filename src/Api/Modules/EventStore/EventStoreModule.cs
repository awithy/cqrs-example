using System.Threading.Tasks;
using Api.Common;

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