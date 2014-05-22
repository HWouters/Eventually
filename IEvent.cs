using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventually
{
    public interface IEvent<TEventArgs>
    {
        IDisposable Subscribe(Action<TEventArgs> eventHandler);
    }

    public interface IEvent<TSender, TEventArgs> : IEvent<TEventArgs>
    {
        IDisposable Subscribe(Action<TSender, TEventArgs> eventHandler);
    }
}
