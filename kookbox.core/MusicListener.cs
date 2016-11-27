using System.Collections.Generic;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    internal class MusicListener : IMusicListener
    {
        private readonly IMusicServer server;
        private readonly List<INetworkTransport> transports = new List<INetworkTransport>();

        public MusicListener(IMusicServer server, INetworkTransport transport, string name)
        {
            this.server = server;
            transports.Add(transport);
             
            Name = name;
        }

        public string Name { get; }
        public bool IsConnected { get; }
        public Option<IMusicRoom> ActiveRoom { get; }
        public Option<IBan> Ban { get; }
        public IEnumerable<IMusicListenerRole> ServerRoles { get; }
        public IEnumerable<INetworkTransport> Transports => transports;
    }
}
