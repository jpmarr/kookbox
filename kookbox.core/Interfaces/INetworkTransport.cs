using System;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface INetworkTransport
    {
        Task RunAsync();
        Task CloseAsync();
        void QueueMessage(INetworkMessage message);
        IObservable<INetworkMessage> ReceivedMessages { get; }
    }
}
