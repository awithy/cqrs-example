using System.Threading.Tasks;
using Api.Common;
using Api.Events;
using Api.Utility;

namespace Api.Modules.Reservations
{
    public interface IReservationsEventHandler : IHandle<SessionCancelledEvent>
    {
    }

    public class ReservationsEventHandler : IReservationsEventHandler
    {
        static Logger Log = new Logger(typeof(ReservationsEventHandler));
        private ReservationsFactory _reservationsFactory;

        public ReservationsEventHandler()
        {
            _reservationsFactory = new ReservationsFactory(EventHandlerContext.Current.EventStore); //this is hacky.  I want to change to use the container.
        }

        public async Task Handle(SessionCancelledEvent @event)
        {
            Log.Debug("Handle SessionCancelledEvent");
        }
    }
}