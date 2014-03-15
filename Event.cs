using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventually
{
    public class Event<TEventArgs> : Event<object, TEventArgs>
    {

    }

    public class Event<TSender, TEventArgs> : IEvent<TSender, TEventArgs>
    {
        private readonly ConcurrentDictionary<Action<TSender, TEventArgs>, Action<TSender, TEventArgs>> eventHandlers = new ConcurrentDictionary<Action<TSender, TEventArgs>, Action<TSender, TEventArgs>>();

        public void Raise(TSender sender, TEventArgs arguments)
        {
            foreach (var handler in eventHandlers.Values)
            {
                handler(sender, arguments);
            }
        }

        public IDisposable Subscribe(Action<TSender, TEventArgs> eventHandler)
        {
            eventHandlers.TryAdd(eventHandler, eventHandler);

            return new Disposable(() =>
            {
                Action<TSender, TEventArgs> handler;
                eventHandlers.TryRemove(eventHandler, out handler);
            });
        }

        private class Disposable : IDisposable
        {
            private readonly Action dispose;

            public Disposable(Action dispose)
            {
                this.dispose = dispose;
            }

            public void Dispose()
            {
                dispose();
            }
        }
    }
}
