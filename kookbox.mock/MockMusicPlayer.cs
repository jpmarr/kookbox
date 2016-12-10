using System;
using System.Reactive.Linq;
using kookbox.core.Interfaces;
using System.Reactive.Subjects;
using kookbox.core;

namespace kookbox.mock
{
    internal class MockMusicPlayer : IPlayer
    {
        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

        private readonly IMusicSource source;
        private readonly string id;
        private readonly string description;
        private ITrack currentTrack;
        private TimeSpan currentPosition;
        private readonly Subject<PlayerEvent> events = new Subject<PlayerEvent>();
        private bool isPlaying;
        private IDisposable subscription;
        private IRoom currentRoom;

        public MockMusicPlayer(IMusicSource source, string id, string description)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            this.source = source;
            this.id = id;
            this.description = description;
        }

        public void Play(ITrack track)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));
            if (track.Source != source)
                throw new ArgumentException("Unsupported track", nameof(track));

            if (currentTrack != track)
            { 
                currentTrack = track;
                currentPosition = TimeSpan.Zero;
            }

            isPlaying = true;
            events.OnNext(PlayerEvent.StartPlaying);
            subscription = Observable
                .Generate(currentPosition, i => i < currentTrack.Duration, i => i + OneSecond, i => i, i => OneSecond)
                .Finally(() => events.OnNext(PlayerEvent.PlaybackComplete))
                .Subscribe(
                    pos =>
                    {
                        currentPosition = pos;
                        events.OnNext(PlayerEvent.PositionChange);
                    });
        }

        public void Stop()
        {
            if (!isPlaying)
                return;

            subscription?.Dispose();
            subscription = null;

            events.OnNext(PlayerEvent.StopPlaying);
            isPlaying = false;
        }

        public void Seek(TimeSpan position)
        {
            throw new NotImplementedException();
        }

        public Option<IRoom> CurrentRoom { get; }

        public Option<ITrack> CurrentTrack => Option.Create(currentTrack);
        public bool CanSeek => false;
        public IObservable<PlayerEvent> Events => events;
        public TimeSpan CurrentPosition => currentPosition;
        public bool IsPlaying => isPlaying;
        public string Id => id;
        public string Description => description;
        public IMusicSource Source => source;

        internal void AssignToRoom(IRoom room)
        {
            currentRoom = room;
        }
    }
}
