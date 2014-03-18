using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eventually
{
    public static class EventExtensions
    {
        public static IDisposable Subscribe<TSender, TEventArgs>(this IEvent<TSender, TEventArgs> eventSource, Action<TEventArgs> eventHandler)
        {
            return eventSource.Subscribe((_, eventArgs) => eventHandler(eventArgs));
        }

        public static IObservable<TEventArgs> ToObservable<TSender, TEventArgs>(this IEvent<TSender, TEventArgs> eventSource)
        {
            return Observable.Create<TEventArgs>(o =>
            {
                return eventSource.Subscribe(o.OnNext);
            });
        }
    }
}
