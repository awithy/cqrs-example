using System.Threading.Tasks;
using Api.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Modules.EventStore
{
    public class EventStoreModule : IInitializable
    {
        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public static void Regiser(IServiceCollection services)
        {
            services.AddSingleton<IEventStore, EventStore>();
        }
    }
}