using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Eventually
{
    public class Bus : IBus
    {
        private readonly ConcurrentDictionary<Type, object> eventSources = new ConcurrentDictionary<Type, object>();

        public IDisposable Subscribe<T>(Action<T> handler)
        {
            var eventSource = GetEventSource<T>();

            return eventSource.Subscribe(handler);
        }

        private IEvent<T> GetEventSource<T>()
        {
            var key = typeof(T);
            if (eventSources.ContainsKey(key))
            {
                return eventSources[key] as Event<T>;
            }
            else
            {
                var eventSource = new Event<T>();
                eventSources.TryAdd(key, eventSource);

                return eventSource;
            }
        }

        public void Publish<T>(T message)
        {
            if (eventSources.ContainsKey(typeof(T)))
            {
                var eventSource = eventSources[typeof(T)] as Event<T>;

                eventSource.Raise(message);
            }
        }
    }
}
