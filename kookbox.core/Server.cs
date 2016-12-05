﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using kookbox.core.Interfaces;
using System.Threading.Tasks;
using kookbox.core.Messaging;
using kookbox.core.Messaging.DTO;

namespace kookbox.core
{
    public class Server : IMusicServer
    {
        private readonly List<IMusicRoom> rooms = new List<IMusicRoom>();
        private readonly List<IMusicListener> connectedListeners = new List<IMusicListener>();
        private readonly IMusicSecurity security = new MusicSecurity();

        public Server()
        {
            //todo: right place for this???
            MessageFactory.RegisterPayloadTypesInAssembly(typeof(NetworkMessage).GetTypeInfo().Assembly);    
        }

        public IMusicSources Sources { get; } = new MusicSources();
        public IEnumerable<IMusicListener> ConnectedListeners => connectedListeners;
        public IEnumerable<IMusicRoom> Rooms => rooms;
        public IMusicSecurity Security => security;

        public Task StartAsync()
        {
            // todo: deserialize any state here    

            // todo: temp - this will loaded from state storage
            var listener = new MusicListener(this, "jim");
            connectedListeners.Add(listener);

            var room = CreateRoomAsync(listener, "Test Room").Result;
            var source = Sources.First();

            IMusicPlaylistSource playlist = null;
            var playlistFactory = source as IMusicPlaylistSourceFactory;
            if (playlistFactory != null)
                playlist = playlistFactory.CreatePlaylistSourceAsync("random").Result;

            room.DefaultTrackSource = Option.Some(playlist);

            room.OpenAsync().Wait();
        }

        public Task StopAsync()
        {
            
        }

        public Task<IMusicRoom> CreateRoomAsync(IMusicListener creator, string name)
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            security.CheckListenerHasPermission(creator, Permission.CreateRoom, Option.Create(this));

            var room = new Room(this, creator, name);
            rooms.Add(room);

            return Task.FromResult<IMusicRoom>(room);
        }

        public Task<IMusicListener> ConnectListenerAsync(string username, INetworkTransport transport)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            IMusicListener listener;
            if (GetListener(username).TryGetValue(out listener))
                listener.AddTransport(transport);
            else
            {
                listener = new MusicListener(this, username, transport);
                security.CheckListenerHasPermission(listener, Permission.Connect, Option.Create(this));
                connectedListeners.Add(listener);
            };

            //todo: get listener default room
            var room = rooms.First();
            room.ConnectListener(listener);

            transport.QueueMessage(MessageFactory.ConnectionResponse(RoomInfo.FromRoom(room)));

            return Task.FromResult(listener);
        }

        public Task<IEnumerable<IMusicListener>> GetListenersAsync(Paging paging)
        {
            throw new NotImplementedException();
        }

        private Option<IMusicListener> GetListener(string username)
        {
            // todo: overall user store
            return Option.Create(connectedListeners.FirstOrDefault(l => l.Name == username));
        }

        private void SendMessageToAllListeners(INetworkMessage message)
        {
            foreach (var listener in connectedListeners)
                foreach (var transport in listener.Transports)
                    transport.QueueMessage(message);
        }
    }
}
