using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicSearchResults
    {
        string SearchCriteria { get; }
        Option<IEnumerable<IMusicTrack>> Tracks { get; }
        Option<IEnumerable<IMusicAlbum>> Albums { get; }
        Option<IEnumerable<IMusicArtist>> Artists { get; }
    }
}
