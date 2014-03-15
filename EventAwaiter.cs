using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eventually
{
    public class EventAwaiter<TSender, TEventArgs> : INotifyCompletion
    {
        private IEvent<TSender, TEventArgs> eventSource;
        private TEventArgs result;
        private bool isCompleted;
        private IDisposable subscription;
        private object gate = new object();

        internal EventAwaiter(IEvent<TSender, TEventArgs> eventSource)
        {
            this.eventSource = eventSource;
        }

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
        }

        public void OnCompleted(Action continuation)
        {
            subscription = this.eventSource.Subscribe(e =>
            {
                lock (gate)
                {
                    subscription.Dispose();
                    if (!isCompleted)
                    {
                        this.result = e;
                        isCompleted = true;
                        continuation();
                    }
                }
            });
        }

        public TEventArgs GetResult()
        {
            return this.result;
        }
    }
}
