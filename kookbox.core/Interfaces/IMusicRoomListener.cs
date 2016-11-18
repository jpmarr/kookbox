using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicRoomListener
    {
        IMusicListener Listener { get; }
        bool IsConnected { get; }
        IEnumerable<IMusicListenerRole> RoomRoles { get; }
        Option<IPoll> Poll { get; }
        Option<IBan> Ban { get; }
    }
}
