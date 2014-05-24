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
        private readonly ConcurrentDictionary<Type, List<object>> subscribers = new ConcurrentDictionary<Type, List<object>>();
        private readonly object subscriptionGate = new object();

        public IDisposable Subscribe<T>(Action<T> handler)
        {
            List<object> handlers = GetHandlers(typeof(T));

            lock (subscriptionGate)
            {
                handlers.Add(handler);
            }

            return Disposable.Create(() =>
            {
                lock (subscriptionGate)
                {
                    handlers.Remove(handler);
                }
            });
        }

        private List<object> GetHandlers(Type key)
        {
            if (subscribers.ContainsKey(key))
            {
                return subscribers[key];
            }
            else
            {
                var handlers = new List<object>();
                subscribers[key] = handlers;

                return handlers;
            }
        }

        public void Publish<T>(T message)
        {
            if (subscribers.ContainsKey(typeof(T)))
            {
                var handlers = subscribers[typeof(T)];

                lock (subscriptionGate)
                {
                    foreach (Action<T> handler in handlers)
                    {
                        handler(message);
                    }
                }
            }
        }
    }
}
