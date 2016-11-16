using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicAlbum
    {
        string Name { get; }
        IMusicArtist Artist { get; }
        IEnumerable<IMusicTrack> Tracks { get; }
    }
}
