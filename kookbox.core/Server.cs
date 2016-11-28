using System;
using System.Collections.Generic;
using System.Reflection;
using kookbox.core.Interfaces;
using System.Threading.Tasks;
using kookbox.core.Messaging;

namespace kookbox.core
{
    public class Server : IMusicServer
    {
        private readonly List<IMusicRoom> rooms = new List<IMusicRoom>();
        private readonly List<IMusicListener> connectedListeners = new List<IMusicListener>();

        public IMusicSources Sources { get; } = new MusicSources();
        public IEnumerable<IMusicListener> ConnectedListeners => connectedListeners;
        public IEnumerable<IMusicRoom> Rooms => rooms;

        public Server()
        {
            //todo: right place for this???
            MessageRegistry.RegisterPayloadTypesInAssembly(typeof(NetworkMessage).GetTypeInfo().Assembly);    
        }

        public void Start()
        {
            // todo: deerialize any state here    
        }

        public Task<IMusicRoom> CreateRoomAsync(IMusicListener creator, string name)
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var room = new Room(this, creator, name);
            rooms.Add(room);

            // todo: room created...
            SendMessageToAllListeners(null);

            return Task.FromResult<IMusicRoom>(room);
        }

        public Task<IMusicListener> ConnectListenerAsync(string username, INetworkTransport transport)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            //todo: find listener?

            var listener = new MusicListener(this, transport, username);
            connectedListeners.Add(listener);

            return Task.FromResult<IMusicListener>(listener);
        }

        public Task<IEnumerable<IMusicListener>> GetListenersAsync(Paging paging)
        {
            throw new NotImplementedException();
        }

        private void SendMessageToAllListeners(INetworkMessage message)
        {
            foreach (var listener in connectedListeners)
                foreach (var transport in listener.Transports)
                    transport.SendMessageAsync(message);
        }
    }
}
