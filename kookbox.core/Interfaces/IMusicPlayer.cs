using System;

namespace kookbox.core.Interfaces
{
    public interface IMusicPlayer
    {
        void Play(IMusicTrack track);
        void Stop();

        void Seek(TimeSpan position);

        IMusicTrack CurrentTrack { get; }
        bool CanSeek { get; }

        IObservable<PlayerEvent> Events { get; }
    }

    public enum PlayerEvent
    {
        StartPlaying,
        StopPlaying,
        PositionChange,
        PlaybackComplete
    }
}
