using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Common;
using Api.Utility;

namespace Api.Modules.EventBus
{
    public interface IEventBus
    {
        Task Initialize();
        Task Publish(EventMessage @event);
    }

    public class EventBus : IEventBus
    {
        private static readonly Logger Log = new Logger(typeof(EventBus));
        private static readonly Dictionary<string, IEnumerable<EventHandler>> EventHandlers = new Dictionary<string, IEnumerable<EventHandler>>();

        public Task Initialize()
        {
            Log.Debug("Initializing...");

            var eventTypes = typeof(EventBus).Assembly
                .DefinedTypes
                .Where(x => x.IsSubclassOf(typeof(EventMessage)));

            foreach (var eventType in eventTypes)
            {
                var handlers = typeof(EventBus).Assembly
                    .DefinedTypes
                    .Where(x => x.IsClass)
                    .Where(x => x.ImplementedInterfaces.Any(y =>
                        y.IsGenericType &&
                        y.GetGenericTypeDefinition() == typeof(IHandle<>)
                        && y.GetGenericArguments().First() == eventType.AsType()
                    ));

                var instances = handlers.Select(Activator.CreateInstance);

                var eventHandlers = instances
                    .Select(x => new EventHandler
                    {
                        MethodInfo = x.GetType().GetMethod("Handle"),
                        Instance = x,
                    });

                EventHandlers.Add(eventType.Name, eventHandlers);
            }

            return Task.CompletedTask;
        }

        public Task Publish(EventMessage @event)
        {
            var eventName = @event.GetType().Name;

            if (!EventHandlers.ContainsKey(eventName))
                return Task.CompletedTask;

            foreach (var handler in EventHandlers[eventName])
                handler.MethodInfo.Invoke(handler.Instance, new[] {@event});

            return Task.CompletedTask;
        }

        private class EventHandler
        {
            public MethodInfo MethodInfo { get; set; }
            public object Instance { get; set; }
        }
    }
}