using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicListener
    {
        string Name { get; }
        bool IsConnected { get; }
        Option<IMusicRoom> ActiveRoom { get; }
        Option<IBan> Ban { get; }
        IEnumerable<IMusicListenerRole> ServerRoles { get; }
    }
}
