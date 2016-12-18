using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using kookbox.core.Interfaces;
using System.Threading.Tasks;
using kookbox.core.Interfaces.Internal;
using kookbox.core.Messaging;
using kookbox.core.Messaging.DTO;

namespace kookbox.core
{
    public class Server : IServerController
    {
        private readonly List<IRoomController> rooms = new List<IRoomController>();
        private readonly List<IUserController> connectedUsers = new List<IUserController>();
        private readonly ISecurity security = new Security();

        public Server()
        {
            //todo: right place for this???
            MessageFactory.RegisterPayloadTypesInAssembly(typeof(NetworkMessage).GetTypeInfo().Assembly);    
        }

        public IMusicSources Sources { get; } = new MusicSources();
        public IEnumerable<IUser> ConnectedUsers => connectedUsers;
        public IEnumerable<IRoom> Rooms => rooms;
        public ISecurity Security => security;

        public async Task StartAsync()
        {
            // todo: deserialize any state here    

            // todo: temp - this will loaded from state storage
            var user = new User(this, "jim") as IUserController;
            connectedUsers.Add(user);

            var room = await (this as IServerController).CreateRoomAsync(user, "Test Room");

            var source = Sources.First();
            IPlaylistSource playlist = null;
            var playlistFactory = source as IPlaylistSourceFactory;
            if (playlistFactory != null)
                playlist = playlistFactory.CreatePlaylistSourceAsync("random").Result;
            room.DefaultTrackSource = Option.Some(playlist);

            var roomUser = await user.ConnectToRoomAsync(room);
            await roomUser.OpenRoomAsync();
        }

        public Task StopAsync()
        {
            return null;
        }

        Task<IRoomController> IServerController.CreateRoomAsync(IUser creator, string name)
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            security.CheckUserHasPermission(creator, Permission.CreateRoom, Option.Create(this));

            var room = new Room(this, creator, name);
            rooms.Add(room);

            return Task.FromResult<IRoomController>(room);
        }

        public async Task<IUser> ConnectUserAsync(string username, INetworkTransport transport)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            IUserController user;
            if (GetUser(username).TryGetValue(out user))
                await user.ConnectAsync(transport);
            else
            {
                user = new User(this, username, transport);
                security.CheckUserHasPermission(user, Permission.Connect, Option.Create(this));
                connectedUsers.Add(user);
            };

            //todo: get listener default room
            var room = rooms.First();
            await user.ConnectToRoomAsync(room);

            return user;
        }

        public Task<IEnumerable<IUser>> GetUsersAsync(Paging paging)
        {
            throw new NotImplementedException();
        }

        private Option<IUserController> GetUser(string username)
        {
            // todo: overall user store
            return Option.Create(connectedUsers.FirstOrDefault(l => l.Name == username));
        }

        private void SendMessageToAllUsers(INetworkMessage message)
        {
            foreach (var listener in connectedUsers)
                foreach (var transport in listener.Transports)
                    transport.QueueMessage(message);
        }

        IRoomController IServerController.GetRoom(string id)
        {
            return rooms.FirstOrDefault(r => r.Id == id);
        }
    }
}
