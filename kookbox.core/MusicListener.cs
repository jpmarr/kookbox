using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    internal class MusicListener : IMusicListener
    {
        private readonly IMusicServer server;
        private readonly List<INetworkTransport> transports = new List<INetworkTransport>();
        private Option<IMusicRoomListener> roomListener;
        private int isDisconnecting;

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
            if (isDisconnecting == 1)
                return;

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

        public bool IsConnected
        {
            get
            {
                if (isDisconnecting == 1)
                    return false;

                lock (transports)
                    return transports.Any();
            }
        }

        public Option<IMusicRoom> ActiveRoom
        {
            get
            {
                IMusicRoomListener rl;
                return roomListener.TryGetValue(out rl) ? 
                    Option.Create(rl.Room) : 
                    Option<IMusicRoom>.None();
            }
        }

        public Option<IBan> Ban { get; }
        public IEnumerable<IMusicListenerRole> ServerRoles { get; }
        public IEnumerable<INetworkTransport> Transports => transports;

        public Task ConnectAsync(INetworkTransport transport)
        {
            if (isDisconnecting == 1)
                throw new InvalidOperationException();

            AddTransport(transport);
            return Task.CompletedTask;
        }

        public Task DisconnectAsync(INetworkTransport transport)
        {
            if (isDisconnecting == 1)
                throw new InvalidOperationException();

            lock (transports)
                transports.Remove(transport);

            (transport as IDisposable)?.Dispose();

            return Task.CompletedTask;
        }

        public async Task DisconnectAsync()
        {
            if (Interlocked.CompareExchange(ref isDisconnecting, 1, 0) == 0)
            {
                // ReSharper disable once InconsistentlySynchronizedField
                var transport = transports.FirstOrDefault();
                while (transport != null)
                {
                    await DisconnectAsync(transport);
                    // ReSharper disable once InconsistentlySynchronizedField
                    transport = transports.FirstOrDefault();
                }
            }
            // disconnect from server/send notification
        }

        public async Task<IMusicRoomListener> ConnectToRoomAsync(IMusicRoom room)
        {
            if (room == null)
                throw new ArgumentNullException();

            IMusicRoomListener rl;
            if (roomListener.TryGetValue(out rl))
            {
                if (room == rl.Room)
                    return rl; //todo: or throw?
                await rl.DisconnectAsync();
            }


            roomListener = Option.Create(r. ConnectListener(this));
        }

        public Task<IMusicRoom> CreateRoomAsync(IMusicListener creator, string name)
        {
            throw new NotImplementedException();
        }

        public Task StartListenerBanPollAsync(IMusicListener listener)
        {
            throw new NotImplementedException();
        }

        public Task StartRoomSwitchPollAsync(IMusicRoom newRoom)
        {
            throw new NotImplementedException();
        }

        public Task StartRoomBanPollAsync(IMusicRoom room)
        {
            throw new NotImplementedException();
        }

        public Task VoteInPollAsync(IPoll poll)
        {
            throw new NotImplementedException();
        }

        private void HandleTransportMessage(INetworkMessage message)
        {
            Debug.WriteLine($"message recvd: {message}");
        }

        private void HandleTransportComplete(INetworkTransport transport)
        {
            Debug.WriteLine($"Detaching transport: {transport}");

            DisconnectAsync(transport);
        }
    }
}
