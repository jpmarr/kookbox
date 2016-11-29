using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface INetworkTransport
    {
        void QueueMessage(INetworkMessage message);
        IObservable<INetworkMessage> ReceivedMessages { get; }
    }
}
