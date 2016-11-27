using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface INetworkTransport
    {
        Task SendMessageAsync(INetworkMessage message);
        IObservable<INetworkMessage> ReceivedMessages { get; }
    }
}
