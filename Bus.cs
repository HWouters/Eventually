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
            var eventSource = AsEvent<T>();

            return eventSource.Subscribe(handler);
        }

        public IEvent<T> AsEvent<T>()
        {
            var key = typeof(T);
            if (eventSources.ContainsKey(key))
            {
                return (Event<T>)eventSources[key];
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
                var eventSource = (Event<T>)eventSources[typeof(T)];

                eventSource.Raise(message);
            }
        }
    }
}
