using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IServer
    {
        IMusicSources Sources { get; }
        IEnumerable<IUser> ConnectedUsers { get; }
        IEnumerable<IRoom> Rooms { get; }
        ISecurity Security { get; }

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
        Task<IUser> ConnectUserAsync(string username, INetworkTransport transport);
        Task<IEnumerable<IUser>> GetUsersAsync(Paging paging);
    }
}
