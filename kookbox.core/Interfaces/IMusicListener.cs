using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicListener
    {
        string Id { get; }
        string Name { get; }
        bool IsConnected { get; }
        Option<IMusicRoom> ActiveRoom { get; }
        Option<IBan> Ban { get; }
        IEnumerable<IMusicListenerRole> ServerRoles { get; }
        IEnumerable<INetworkTransport> Transports { get; }

        Task ConnectAsync(INetworkTransport transport);
        Task DisconnectAsync(INetworkTransport transport);
        Task DisconnectAsync();

        Task<IMusicRoomListener> ConnectToRoomAsync(Option<IMusicRoom> room);
        Task<IMusicRoom> CreateRoomAsync(IMusicListener creator, string name);

        Task StartListenerBanPollAsync(IMusicListener listener);
        Task StartRoomSwitchPollAsync(IMusicRoom newRoom);
        Task StartRoomBanPollAsync(IMusicRoom room);

        Task VoteInPollAsync(IPoll poll);
    }
}
