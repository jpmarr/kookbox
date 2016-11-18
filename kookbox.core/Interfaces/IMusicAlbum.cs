using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicAlbum
    {
        string Name { get; }
        IMusicArtist Artist { get; }
        IEnumerable<IMusicTrack> Tracks { get; }
    }
}
