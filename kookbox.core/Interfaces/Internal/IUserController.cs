using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces.Internal
{
    internal interface IUserController : IUser
    {
        IEnumerable<INetworkTransport> Transports { get; }

        Task ConnectAsync(INetworkTransport transport);
        Task DisconnectAsync(INetworkTransport transport);
        Task DisconnectAsync();

        void QueueTransportMessage(INetworkMessage message);
    }
}
