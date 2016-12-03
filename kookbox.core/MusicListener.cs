using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            AddTransport(transport);
        }

        public void AddTransport(INetworkTransport transport)
        {
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            transport.ReceivedMessages.Subscribe(
                HandleTransportMessage,
                () => HandleTransportComplete(transport));

            lock (transports)
                transports.Add(transport);
        }

        public string Id { get; }
        public string Name { get; }
        public bool IsConnected => transports.Any();
        public Option<IMusicRoom> ActiveRoom { get; set; }
        public Option<IBan> Ban { get; }
        public IEnumerable<IMusicListenerRole> ServerRoles { get; }
        public IEnumerable<INetworkTransport> Transports => transports;

        private void HandleTransportMessage(INetworkMessage message)
        {
            Debug.WriteLine($"message recvd: {message}");
        }

        private void HandleTransportComplete(INetworkTransport transport)
        {
            Debug.WriteLine($"Detaching transport: {transport}");
            lock (transports)
                transports.Remove(transport);

            (transport as IDisposable)?.Dispose();
        }
    }
}
