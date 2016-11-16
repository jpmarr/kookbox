using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicTrack
    {
        IMusicSource Source { get; }
        string Title { get; }
        int Number { get; }
        IMusicArtist Artist { get; }
        IMusicAlbum Album { get; }
        TimeSpan Duration { get; }
        IMusicListener Introducer { get; }
        Option<IBan> Ban { get; }
    }
}
