using System;
using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicAlbum
    {
        string Id { get; }
        string Name { get; }
        IMusicArtist Artist { get; }
        IEnumerable<IMusicTrack> Tracks { get; }
        Uri ImageUri { get; }
    }
}
