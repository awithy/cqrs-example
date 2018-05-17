using System.Threading.Tasks;
using Api.Common;
using Api.Modules.EventStore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Modules.Reservations
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReservationsModule : IInitializable
    {
        private readonly IEventStore _eventStore;

        public ReservationsModule(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<IInitializable, ReservationsModule>();
            services.AddSingleton<IReservationsFactory, ReservationsFactory>();
            services.AddSingleton<IReservationsEventHandler, ReservationsEventHandler>();
        }
    }
}