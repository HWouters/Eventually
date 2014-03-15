using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventually
{
    public static class EventExtensions
    {
        public static IDisposable Subscribe<TSender, TEventArgs>(this IEvent<TSender, TEventArgs> eventSource, Action<TEventArgs> eventHandler)
        {
            return eventSource.Subscribe((_, eventArgs) => eventHandler(eventArgs));
        }

        public static IEvent<TSender, TResult> Select<TSender, TSource, TResult>(this IEvent<TSender, TSource> sourceEvent, Func<TSource, TResult> selector)
        {
            return new EventWrapper<TSender, TResult>(eventHandler =>
            {
                return sourceEvent.Subscribe((sender, eventArgs) => eventHandler(sender, selector(eventArgs)));
            });
        }

        public static IEvent<TSender, T> Where<TSender, T>(this IEvent<TSender, T> sourceEvent, Func<T, bool> filter)
        {
            return new EventWrapper<TSender, T>(eventHandler =>
            {
                return sourceEvent.Subscribe((sender, eventArgs) =>
                {
                    if (filter(eventArgs))
                    {
                        eventHandler(sender, eventArgs);
                    }
                });
            });
        }

        public static EventAwaiter<TSender, TEventArgs> GetAwaiter<TSender, TEventArgs>(this IEvent<TSender, TEventArgs> eventSource)
        {
            TaskCompletionSource<TEventArgs> tcs = new TaskCompletionSource<TEventArgs>();

            return new EventAwaiter<TSender, TEventArgs>(eventSource);
        }

        private class EventWrapper<TSender, TResult> : IEvent<TSender, TResult>
        {
            private readonly Func<Action<TSender, TResult>, IDisposable> onSubscribe;

            public EventWrapper(Func<Action<TSender, TResult>, IDisposable> onSubscribe)
            {
                this.onSubscribe = onSubscribe;
            }

            public IDisposable Subscribe(Action<TSender, TResult> eventHandler)
            {
                return onSubscribe(eventHandler);
            }
        }
    }
}
