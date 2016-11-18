using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicServer
    {
        IMusicEventBus EventBus { get; }
        IMusicSources Sources { get; }
        IEnumerable<IMusicListener> ConnectedListeners { get; }
        IEnumerable<IMusicRoom> Rooms { get; }

        IMusicRoom CreateRoom(IMusicListener creator, string name);

        IMusicListener ConnectListener(string username);
        IEnumerable<IMusicListener> GetListeners(Paging paging);
    }
}
