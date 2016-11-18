using System;
using System.Collections.Generic;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    public class Server : IMusicServer
    {
        public IMusicSources Sources { get; } = new MusicSources();
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

        public IMusicRoom CreateRoom(IMusicListener creator, string name)
        {
            throw new NotImplementedException();
        }

        public IMusicListener ConnectListener(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMusicListener> GetListeners(Paging paging)
        {
            throw new NotImplementedException();
        }
    }
}
