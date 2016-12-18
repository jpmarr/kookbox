using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Interfaces.Internal;
using kookbox.core.Interfaces.Serialisation;
using kookbox.core.Messaging;

namespace kookbox.core
{
    internal class Room : IRoomController
    {
        private readonly int upcomingMinimumCount = 20;

        private readonly IServerController server;
        private readonly ISecurity security = new Security();
        private readonly Dictionary<string, IRoomUserController> roomUsers = new Dictionary<string, IRoomUserController>();
        private readonly ITrackQueue queue = new TrackQueue();
        private IDisposable playerEventSubscription;
        private Option<IQueuedTrack> currentTrack = Option<IQueuedTrack>.None();
        private readonly Dictionary<IMusicSource, IPlayer> players = new Dictionary<IMusicSource, IPlayer>();

        private Room(IServerController server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            this.Id = Guid.NewGuid().ToString();
            this.server = server;
        }

        public Room(IServerController server, IRoomDeserialiser deserialiser)
            : this(server)
        {
            if (deserialiser == null)
                throw new ArgumentNullException(nameof(deserialiser));

            // load state from deserialiser
        }

        /// <summary>
        /// Creating a new room
        /// </summary>
        /// <param name="server"></param>
        /// <param name="creator"></param>
        /// <param name="name"></param>
        public Room(IServerController server, IUser creator, string name)
            : this(server)
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("no name", nameof(name));

            Creator = creator;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
        public IUser Creator { get; }
        public ISecurity Security { get; }
        
        //todo: multiple sources??? legal to have no -source from a 'request-only' room?
        public Option<IPlaylistSource> DefaultTrackSource { get; set; }

        public Option<IQueuedTrack> CurrentTrack
        {
            get { return currentTrack; }
            set
            {
                // if playing, stop
                CurrentTrack.IfHasValue(async _ => await ((IRoomController)this).PauseAsync());

                currentTrack = value;
                // if track is still set and we were playing, play the new track

            }
        }

        public ITrackQueue UpcomingQueue => queue;
        public IEnumerable<IRoomUser> Users => roomUsers.Values;

        public RoomState State
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IRoomUserController IRoomController.ConnectUser(IUserController newUser)
        {
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            security.CheckUserHasPermission(newUser, Permission.Connect, Option.Create(this));

            // add this user if they're not already in the list
            IRoomUserController roomUser;
            lock (roomUsers)
            { 
                if (!roomUsers.TryGetValue(newUser.Id, out roomUser))
                {
                    roomUser = new RoomUser(this, newUser);
                    roomUsers.Add(newUser.Id, roomUser);
                }
            }

            return roomUser;
        }

        void IRoomController.DisconnectUser(IRoomUser roomUser)
        {
            if (roomUser.Room != this)
                throw new ArgumentException("user is not for this room");

            lock (roomUsers)
                roomUsers.Remove(roomUser.User.Id);
        }

        public IEnumerable<IQueuedTrack> GetTrackHistory(int count)
        {
            throw new NotImplementedException();
        }

        async Task IRoomController.PlayAsync()
        {
            CurrentTrack.IfHasValue(async track => await PlayTrackAsync(track));   
        }

        async Task IRoomController.PauseAsync()
        {
            CurrentTrack.IfHasValue(async track => await PauseTrackAsync(track));    
        }

        async Task IRoomController.OpenAsync()
        {
            if (!CurrentTrack.HasValue)
                await InitialiseRoom();

            await ((IRoomController)this).PlayAsync();
        }

        async Task IRoomController.CloseAsync()
        {
            await ((IRoomController)this).PauseAsync();
        }

        private async Task PlayTrackAsync(IQueuedTrack queued)
        {
            var player = await GetPlayerForSourceAsync(queued.Track.Source);
            player
                .IfHasValue(p => p.Play(queued.Track))
                .Else(() =>
                {
                    throw new InvalidOperationException();
                });
        }

        private async Task<Option<IPlayer>> GetPlayerForSourceAsync(IMusicSource source)
        {
            // todo: may have specific player preference here
            IPlayer player;
            if (!players.TryGetValue(source, out player))
            {
                var sourcePlayer = await source.RequestAvailablePlayerAsync(this);
                sourcePlayer.IfHasValue(p =>
                {
                    players.Add(source, p);
                    SubscribePlayerEvents(p);
                });
                return sourcePlayer;
            }
            return Option.Create(player);
        }

        private void SubscribePlayerEvents(IPlayer player)
        {
            playerEventSubscription?.Dispose();
            playerEventSubscription = player.Events.Subscribe(async evt => await HandlePlayerEventAsync(evt));
        }

        private async Task HandlePlayerEventAsync(PlayerEvent playerEvent)
        {
            switch (playerEvent)
            {
                case PlayerEvent.StartPlaying:
                    CurrentTrack.IfHasValue(HandlePlaybackStarted);
                    break;
                case PlayerEvent.StopPlaying:
                    Console.WriteLine("Stop playing");
                    break;
                case PlayerEvent.PositionChange:
                    CurrentTrack.IfHasValue(async t =>
                    {
                        var player = await GetPlayerForSourceAsync(t.Track.Source);
                        player.IfHasValue(p => Console.WriteLine($"Position change: {p.CurrentPosition}"));
                    });
                    
                    break;
                case PlayerEvent.PlaybackComplete:
                    CurrentTrack.IfHasValue(t => Console.WriteLine($"Completed Track: \"{t.Track.Title}"));
                    await CueNextTrackAsync();
                    await ((IRoomController)this).PlayAsync();
                    break;
            }  
        }

        private void HandlePlaybackStarted(IQueuedTrack queued)
        {
            Console.WriteLine($"Start Track: \"{queued.Track.Title}\" - Duration: {queued.Track.Duration}");
            SendMessageToAllListeners(MessageFactory.TrackStarted(this, queued.Track));
        }

        private async Task PauseTrackAsync(IQueuedTrack queued)
        {
            var player = await GetPlayerForSourceAsync(queued.Track.Source);
            player.IfHasValue(p => p.Stop());
        }

        private async Task InitialiseRoom()
        {
            await FillQueue();
            if (!CurrentTrack.HasValue)
                await CueNextTrackAsync();
        }

        private async Task CueNextTrackAsync()
        {
            CurrentTrack.IfHasValue(AddToHistory);
            CurrentTrack = UpcomingQueue.DequeueNextTrack();
            await FillQueue();
        }

        private async Task FillQueue()
        {
            // populate the queue if necessary
            DefaultTrackSource.IfHasValue(async trackSource =>
            {
                while (!trackSource.IsExhausted && UpcomingQueue.Count < upcomingMinimumCount)
                {
                    var nextTrack = await trackSource.GetNextTrackAsync();
                    nextTrack.IfHasValue(track => UpcomingQueue.QueueTrack(track));
                }
            });
        }

        private void AddToHistory(IQueuedTrack track)
        {
            
        }

        private void SendMessageToAllListeners(INetworkMessage message)
        {
            foreach (var roomUser in roomUsers.Values)
                roomUser.UserController.QueueTransportMessage(message);
        }
    }
}
