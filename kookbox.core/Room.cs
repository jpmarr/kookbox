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
    // todo: remove all actions from the room itself and move to room listener so 
    // they are all done in the context of a specific user only - this allows for cleaner security management
    internal class Room : IMusicRoomController
    {
        private readonly IMusicServer server;
        private readonly IMusicSecurity security = new MusicSecurity();
        private readonly int upcomingMinimumCount = 20;
        private readonly List<IMusicRoomListener> listeners = new List<IMusicRoomListener>();
        private readonly IMusicQueue queue = new MusicQueue();
        private IDisposable playerEventSubscription;
        private Option<IQueuedMusicTrack> currentTrack = Option<IQueuedMusicTrack>.None();
        private readonly Dictionary<IMusicSource, IMusicPlayer> players = new Dictionary<IMusicSource, IMusicPlayer>();

        private Room(IMusicServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            this.server = server;
        }

        public Room(IMusicServer server, IMusicRoomDeserialiser deserialiser)
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
        public Room(IMusicServer server, IMusicListener creator, string name)
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
        public IMusicListener Creator { get; }
        public IMusicSecurity Security { get; }
        
        //todo: multiple sources??? legal to have no -source from a 'request-only' room?
        public Option<IMusicPlaylistSource> DefaultTrackSource { get; set; }

        public Option<IQueuedMusicTrack> CurrentTrack
        {
            get { return currentTrack; }
            set
            {
                // if playing, stop
                CurrentTrack.IfHasValue(async _ => await PauseAsync());

                currentTrack = value;
                // if track is still set and we were playing, play the new track

            }
        }

        public IMusicQueue UpcomingQueue => queue;
        public IEnumerable<IMusicRoomListener> Listeners => listeners;

        public RoomState State
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IMusicRoomListener ConnectListener(IMusicListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            security.CheckListenerHasPermission(listener, Permission.Connect, Option.Create(this));

            // add this listener if they're not already in the list
            IMusicRoomListener roomListener;
            lock (listeners)
                roomListener = listeners.FirstOrDefault(l => l.Listener == listener);

            if (roomListener == null)
            {
                roomListener = new RoomListener(this, this, listener);
                lock (listeners)
                    listeners.Add(roomListener);
            }

            return roomListener;
        }

        public void DisconnectListener(IMusicRoomListener roomListener)
        {
            if (roomListener.Room != this)
                throw new ArgumentException("listener is not for this room");

            lock (listeners)
                listeners.Remove(roomListener);

            roomListener.Listener.ConnectToRoom(Option<IMusicRoom>.None());
        }

        public async Task PlayAsync()
        {
            CurrentTrack.IfHasValue(async track => await PlayTrackAsync(track));   
        }

        public async Task PauseAsync()
        {
            CurrentTrack.IfHasValue(async track => await PauseTrackAsync(track));    
        }

        public IEnumerable<IQueuedMusicTrack> GetTrackHistory(int count)
        {
            throw new NotImplementedException();
        }

        async Task IMusicRoomController.OpenAsync()
        {
            if (!CurrentTrack.HasValue)
                await InitialiseRoom();

            await PlayAsync();
        }

        async Task IMusicRoomController.CloseAsync()
        {
            await PauseAsync();
        }

        private async Task PlayTrackAsync(IQueuedMusicTrack queued)
        {
            var player = await GetPlayerForSourceAsync(queued.Track.Source);
            player
                .IfHasValue(p => p.Play(queued.Track))
                .Else(() =>
                {
                    throw new InvalidOperationException();
                });
        }

        private async Task<Option<IMusicPlayer>> GetPlayerForSourceAsync(IMusicSource source)
        {
            // todo: may have specific player preference here
            IMusicPlayer player;
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

        private void SubscribePlayerEvents(IMusicPlayer player)
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
                    await PlayAsync();
                    break;
            }  
        }

        private void HandlePlaybackStarted(IQueuedMusicTrack queued)
        {
            Console.WriteLine($"Start Track: \"{queued.Track.Title}\" - Duration: {queued.Track.Duration}");
            SendMessageToAllListeners(MessageFactory.TrackStarted(this, queued.Track));
        }

        private async Task PauseTrackAsync(IQueuedMusicTrack queued)
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

        private void AddToHistory(IQueuedMusicTrack track)
        {
            
        }

        private void SendMessageToAllListeners(INetworkMessage message)
        {
            foreach (var roomListener in listeners)
                foreach (var transport in roomListener.Listener.Transports)
                    transport.QueueMessage(message);
        }
    }
}
