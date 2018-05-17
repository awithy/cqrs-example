using System.Threading.Tasks;
using Api.Common;
using Api.Modules.EventStore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Modules.EventBus
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EventBusModule : IInitializable
    {
        private readonly IEventBus _eventBus;
        private readonly IEventStore _eventStore;

        public EventBusModule(IEventBus eventBus, IEventStore eventStore)
        {
            _eventBus = eventBus;
            _eventStore = eventStore;
        }

        public async Task Initialize()
        {
            EventHandlerContext.Current = new EventHandlerContext // this is hacky. I want to change this to use the container
            {
                EventStore = _eventStore,
                EventBus = _eventBus,
            };
            await _eventBus.Initialize(_eventStore);
        }

        public static void Regiser(IServiceCollection services)
        {
            services.AddSingleton<IInitializable, EventBusModule>();
            services.AddSingleton<IEventBus, EventBus>();
        }
    }
}