using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
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
