using System;
using System.Collections.Generic;
using kookbox.core.Interfaces;
using System.Threading.Tasks;

namespace kookbox.core
{
    public class Server : IMusicServer
    {
        public IMusicSources SourceSet { get; } = new MusicSources();
        public INetworkTransports Transports { get; } = new NetworkTransports();
        public IEnumerable<IMusicListener> ConnectedListeners { get; }
        public IEnumerable<IMusicRoom> Rooms { get; }

        public IMusicEventBus EventBus
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task<IMusicRoom> CreateRoomAsync(IMusicListener creator, string name)
        {
            throw new NotImplementedException();
        }

        public Task<IMusicListener> ConnectListenerAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IMusicListener>> GetListenersAsync(Paging paging)
        {
            throw new NotImplementedException();
        }
    }
}
