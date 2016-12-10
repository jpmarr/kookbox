using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Interfaces.Internal;

namespace kookbox.core
{
    internal class User : IUser
    {
        private readonly IServerController server;
        private readonly List<INetworkTransport> transports = new List<INetworkTransport>();
        private Option<IRoomUser> roomUser;
        private int isDisconnecting;

        public User(IServerController server, string name)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.server = server;
            Name = name;
        }

        public User(IServerController server, string name, INetworkTransport transport)
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

        public Option<IRoom> ActiveRoom
        {
            get
            {
                IRoomUser ru;
                return roomUser.TryGetValue(out ru) ? 
                    Option.Create(ru.Room) : 
                    Option<IRoom>.None();
            }
        }

        public Option<IBan> Ban { get; }
        public IEnumerable<IUserRole> ServerRoles { get; }
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

        public async Task<IRoomUser> ConnectToRoomAsync(IRoom room)
        {
            if (room == null)
                throw new ArgumentNullException();

            IRoomUser ru;
            if (roomUser.TryGetValue(out ru))
            {
                // if we're already connected to this room, return the existing room listener
                if (room == ru.Room)
                    return ru; //todo: or throw?
                // if we're changing rooms, dosconnect from the current room
                await ru.DisconnectAsync();
            }

            ru = server.GetRoom(room.Id)?.ConnectUser(this);
            roomUser = Option.Create(ru);

            return ru;
        }

        public Task<IRoom> CreateRoomAsync(IUser creator, string name)
        {
            throw new NotImplementedException();
        }

        public Task StartListenerBanPollAsync(IUser listener)
        {
            throw new NotImplementedException();
        }

        public Task StartRoomSwitchPollAsync(IRoom newRoom)
        {
            throw new NotImplementedException();
        }

        public Task StartRoomBanPollAsync(IRoom room)
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
