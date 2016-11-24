using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace kookbox.mock
{
    internal class MockMusicPlayer : IMusicPlayer
    {
        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

        private readonly IMusicSource source;
        private IMusicTrack currentTrack;
        private TimeSpan currentPosition;
        private readonly Subject<PlayerEvent> events = new Subject<PlayerEvent>();
        private bool isPlaying;
        private IDisposable subscription;

        public MockMusicPlayer(IMusicSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            this.source = source;
        }

        public void Play(IMusicTrack track)
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
                .Subscribe(
                    pos =>
                    {
                        currentPosition = pos;
                        events.OnNext(PlayerEvent.PositionChange);
                    },
                    _ => events.OnNext(PlayerEvent.PlaybackComplete));
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

        public IMusicTrack CurrentTrack => currentTrack;
        public bool CanSeek => false;
        public IObservable<PlayerEvent> Events => events;
        public TimeSpan CurrentPosition => currentPosition;
        public bool IsPlaying => isPlaying;
    }
}
