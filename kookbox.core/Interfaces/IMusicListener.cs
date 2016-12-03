using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicListener
    {
        string Id { get; }
        string Name { get; }
        bool IsConnected { get; }
        Option<IMusicRoom> ActiveRoom { get; set; }
        Option<IBan> Ban { get; }
        IEnumerable<IMusicListenerRole> ServerRoles { get; }
        // user may have multiple connections with different trasnports (ie multiple browser sessions or browser + API connection)
        IEnumerable<INetworkTransport> Transports { get; }

        void AddTransport(INetworkTransport transport);
    }
}
