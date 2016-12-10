using System;

namespace kookbox.core.Interfaces
{
    public interface IPlayer : IPlayerDescriptor
    {
        void Play(ITrack track);
        void Stop();

        void Seek(TimeSpan position);

        Option<ITrack> CurrentTrack { get; }
        TimeSpan CurrentPosition { get; }
        bool IsPlaying { get; }
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
