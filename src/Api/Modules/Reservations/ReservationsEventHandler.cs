using System.Threading.Tasks;
using Api.Events;

namespace Api.Modules.Reservations
{
    public class ReservationsEventHandler : IHandle<SessionLocationChanged>
    {
        static Logger Log = new Logger(typeof(ReservationsEventHandler));

        public Task Handle(SessionLocationChanged @event)
        {
            Log.Debug("Handle SessionLocationChanged");
            return Task.CompletedTask;
        }
    }
}