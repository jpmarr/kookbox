using System;

namespace kookbox.core.Interfaces
{
    public interface ITrack
    {
        string Id { get; }
        IMusicSource Source { get; }
        string Title { get; }
        int Number { get; }
        IArtist Artist { get; }
        Option<IAlbum> Album { get; }
        TimeSpan Duration { get; }
        IUser Introducer { get; }
        Option<IBan> Ban { get; }
    }
}
