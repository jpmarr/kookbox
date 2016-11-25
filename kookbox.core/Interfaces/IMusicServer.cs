using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicServer
    {
        IMusicEventBus EventBus { get; }
        IMusicSources Sources { get; }
        IEnumerable<IMusicListener> ConnectedListeners { get; }
        IEnumerable<IMusicRoom> Rooms { get; }

        Task<IMusicRoom> CreateRoomAsync(IMusicListener creator, string name);
        Task<IMusicListener> ConnectListenerAsync(string username);
        Task<IEnumerable<IMusicListener>> GetListenersAsync(Paging paging);
    }
}
