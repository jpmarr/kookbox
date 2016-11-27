using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicServer
    {
        IMusicSources Sources { get; }
        IEnumerable<IMusicListener> ConnectedListeners { get; }
        IEnumerable<IMusicRoom> Rooms { get; }

        void Start();

        Task<IMusicRoom> CreateRoomAsync(IMusicListener creator, string name);
        /// <summary>
        /// connect a new user or establish a new network transport for an already connected listener
        /// </summary>
        /// <param name="username"></param>
        /// <param name="transport"></param>
        /// <returns></returns>
        Task<IMusicListener> ConnectListenerAsync(string username, INetworkTransport transport);
        Task<IEnumerable<IMusicListener>> GetListenersAsync(Paging paging);
    }
}
