using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    internal class MusicListener : IMusicListener
    {
        private readonly IMusicServer server;
        private readonly List<INetworkTransport> transports = new List<INetworkTransport>();

        public MusicListener(IMusicServer server, string name)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.server = server;
            Name = name;
        }

        public MusicListener(IMusicServer server, string name, INetworkTransport transport)
            : this(server, name)
        {
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            transports.Add(transport);
        }

        public void AddTransport(INetworkTransport transport)
        {
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            transports.Add(transport);
        }

        public string Name { get; }
        public bool IsConnected => transports.Any();
        public Option<IMusicRoom> ActiveRoom { get; }
        public Option<IBan> Ban { get; }
        public IEnumerable<IMusicListenerRole> ServerRoles { get; }
        public IEnumerable<INetworkTransport> Transports => transports;
    }
}
