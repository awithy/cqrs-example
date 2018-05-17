using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Modules.EventBus
{
    public class EventBusModule : IInitializable
    {
        private readonly IEventBus _eventBus;

        public EventBusModule(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task Initialize()
        {
            await _eventBus.Initialize();
        }

        public static void Regiser(IServiceCollection services)
        {
            services.AddSingleton<IInitializable, EventBusModule>();
            services.AddSingleton<IEventBus, EventBus>();
        }
    }
}