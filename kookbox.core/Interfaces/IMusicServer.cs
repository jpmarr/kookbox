using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicServer
    {
        IMusicSources Sources { get; }
        IEnumerable<IMusicListener> ConnectedListeners { get; }
        IEnumerable<IMusicRoom> Rooms { get; }
        IMusicSecurity Security { get; }

        // todo: shared/public playback 'zones' - registered by sources (may be multiple sonos zones for example)
        // a room can be asigned to a zone - vote to switch the room in a zone etc.

        Task StartAsync();
        Task StopAsync();

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
