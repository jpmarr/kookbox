using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicServer
    {
        IMusicSources Sources { get; }
        IEnumerable<IMusicListener> ConnectedListeners { get; }
        IEnumerable<IMusicRoom> Rooms { get; }

        IMusicRoom CreateRoom(IMusicListener creator, string name);

        IMusicListener ConnectListener(string username);
        IEnumerable<IMusicListener> GetListeners(Paging paging);
    }
}
