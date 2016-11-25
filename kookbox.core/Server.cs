using System;
using System.Collections.Generic;
using kookbox.core.Interfaces;
using System.Threading.Tasks;

namespace kookbox.core
{
    public class Server : IMusicServer
    {
        private readonly List<IMusicRoom> rooms = new List<IMusicRoom>();
        private readonly List<IMusicListener> connectedListeners = new List<IMusicListener>();

        public IMusicSources Sources { get; } = new MusicSources();
        public INetworkTransports Transports { get; } = new NetworkTransports();
        public IEnumerable<IMusicListener> ConnectedListeners => connectedListeners;
        public IEnumerable<IMusicRoom> Rooms => rooms;

        public IMusicEventBus EventBus
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task<IMusicRoom> CreateRoomAsync(IMusicListener creator, string name)
        {
            var room = new Room(this, creator, name);
            rooms.Add(room);

            return Task.FromResult<IMusicRoom>(room);
        }

        public Task<IMusicListener> ConnectListenerAsync(string username)
        {
            var listener = new MusicListener(this, username);
            connectedListeners.Add(listener);

            return Task.FromResult<IMusicListener>(listener);
        }

        public Task<IEnumerable<IMusicListener>> GetListenersAsync(Paging paging)
        {
            throw new NotImplementedException();
        }
    }
}
