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
        public static IObservable<TEventArgs> ToObservable<TEventArgs>(this IEvent<TEventArgs> eventSource)
        {
            return Observable.Create<TEventArgs>(o =>
            {
                return eventSource.Subscribe(o.OnNext);
            });
        }
    }
}
