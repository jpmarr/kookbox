using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Interfaces.Serialisation;

namespace kookbox.core
{
    public class Room : IMusicRoom
    {
        private readonly IMusicServer server;
        private readonly int upcomingMinimumCount;
        private readonly List<IMusicRoomListener> listeners = new List<IMusicRoomListener>();
        private IDisposable playerEventSubscription;
        private Option<IQueuedMusicTrack> currentTrack = Option<IQueuedMusicTrack>.None();

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

            listeners.Add(new RoomListener(this, creator));
        }

        public string Name { get; }
        public IMusicListener Creator { get; }
        
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

        public IMusicQueue UpcomingQueue { get; }
        public IEnumerable<IMusicRoomListener> Listeners => listeners;

        public RoomState State
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public async Task OpenAsync()
        {
            if (!CurrentTrack.HasValue)
                await InitialiseRoom();

            await PlayAsync();
        }

        public async Task CloseAsync()
        {
            await PauseAsync();
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

        private async Task PlayTrackAsync(IQueuedMusicTrack queued)
        {
            var player = queued.Track.Source.Player;

            SubscribePlayerEvents(player);

            player.Play(queued.Track);
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
                    break;
                case PlayerEvent.StopPlaying:
                    break;
                case PlayerEvent.PositionChange:
                    break;
                case PlayerEvent.PlaybackComplete:
                    await CueNextTrackAsync();
                    await PlayAsync();
                    break;
            }  
        }

        private async Task PauseTrackAsync(IQueuedMusicTrack queued)
        {
            var player = queued.Track.Source.Player;

            player.Stop();
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
                while (UpcomingQueue.Count < upcomingMinimumCount)
                    UpcomingQueue.QueueTrack(await trackSource.GetNextTrackAsync());
            });
        }

        private void AddToHistory(IQueuedMusicTrack track)
        {
            
        }
    }
}
