using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface ISearchResults
    {
        string SearchCriteria { get; }
        Option<IEnumerable<ITrack>> Tracks { get; }
        Option<IEnumerable<IAlbum>> Albums { get; }
        Option<IEnumerable<IArtist>> Artists { get; }
    }
}
