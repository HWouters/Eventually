using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Eventually
{
    public class Event<T> : IEvent<T>
    {
        private readonly ConcurrentDictionary<Action<T>, Action<T>> eventHandlers = new ConcurrentDictionary<Action<T>, Action<T>>();

        private T cachedArguments;
        private bool hasCachedArguments;

        public void Raise(T arguments)
        {
            cachedArguments = arguments;
            hasCachedArguments = true;

            foreach (var handler in eventHandlers.Values)
            {
                handler(arguments);
            }
        }

        public IDisposable Subscribe(Action<T> eventHandler)
        {
            eventHandlers.TryAdd(eventHandler, eventHandler);
            if (hasCachedArguments)
            {
                eventHandler(cachedArguments);
            }

            return Disposable.Create(() =>
            {
                Action<T> handler;
                eventHandlers.TryRemove(eventHandler, out handler);
            });
        }
    }
}
