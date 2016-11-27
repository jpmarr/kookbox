using System;

namespace kookbox.core.Interfaces
{
    public interface IMusicTrack
    {
        string Id { get; }
        IMusicSource Source { get; }
        string Title { get; }
        int Number { get; }
        IMusicArtist Artist { get; }
        Option<IMusicAlbum> Album { get; }
        TimeSpan Duration { get; }
        IMusicListener Introducer { get; }
        Option<IBan> Ban { get; }
    }
}
