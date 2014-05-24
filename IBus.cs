using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventually
{
    public interface IBus
    {
        IDisposable Subscribe<T>(Action<T> handler);

        void Publish<T>(T message);
    }
}
