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

        public void Raise(T arguments)
        {
            foreach (var handler in eventHandlers.Values)
            {
                handler(arguments);
            }
        }

        public IDisposable Subscribe(Action<T> eventHandler)
        {
            eventHandlers.TryAdd(eventHandler, eventHandler);

            return Disposable.Create(() =>
            {
                Action<T> handler;
                eventHandlers.TryRemove(eventHandler, out handler);
            });
        }
    }
}
