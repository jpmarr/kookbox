using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Interfaces.Serialisation;

namespace kookbox.core
{
    public class Room : IMusicRoom
    {
        private readonly IMusicServer server;
        private readonly int upcomingMinimumCount;

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
        }

        public string Name { get; }
        public IMusicListener Creator { get; }
        
        //todo: multiple sources???
        public IMusicPlaylistSource DefaultTrackSource { get; }

        public Option<IQueuedMusicTrack> CurrentTrack
        {
            get { return currentTrack; }
            set
            {
                // if playing, stop
                CurrentTrack.IfHasValue(_ => Pause());

                currentTrack = value;
                // if track is still set and we were playing, play the new track

            }
        } 

        public IMusicQueue UpcomingQueue { get; }
        public IEnumerable<IMusicRoomListener> Listeners { get; }

        public RoomState State
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Open()
        {
            if (!CurrentTrack.HasValue)
                InitialiseRoom();

            Play();
        }

        public void Close()
        {
            Pause();
        }

        public void Play()
        {
            CurrentTrack.IfHasValue(PlayTrack);   
        }

        public void Pause()
        {
            CurrentTrack.IfHasValue(PauseTrack);    
        }

        public IEnumerable<IQueuedMusicTrack> GetTrackHistory(int count)
        {
            throw new NotImplementedException();
        }

        private void PlayTrack(IQueuedMusicTrack queued)
        {
            var player = queued.Track.Source.Player;

            SubscribePlayerEvents(player);

            player.Play(queued.Track);
        }

        private void SubscribePlayerEvents(IMusicPlayer player)
        {
            playerEventSubscription?.Dispose();
            playerEventSubscription = player.Events.Subscribe(HandlePlayerEvent);
        }

        private void HandlePlayerEvent(PlayerEvent playerEvent)
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
                    CueNextTrack();
                    Play();
                    break;
            }  
        }

        private void PauseTrack(IQueuedMusicTrack queued)
        {
            var player = queued.Track.Source.Player;

            player.Stop();
        }

        private void InitialiseRoom()
        {
            FillQueue();
            if (!CurrentTrack.HasValue)
                CueNextTrack();
        }

        private void CueNextTrack()
        {
            CurrentTrack.IfHasValue(AddToHistory);

            CurrentTrack = UpcomingQueue.DequeueNextTrack();
            FillQueue();
        }

        private void FillQueue()
        {
            // populate the queue if necessary
            while (UpcomingQueue.Count < upcomingMinimumCount)
                UpcomingQueue.QueueTrack(DefaultTrackSource.GetNextTrack());
        }

        private void AddToHistory(IQueuedMusicTrack track)
        {
            
        }
    }
}
