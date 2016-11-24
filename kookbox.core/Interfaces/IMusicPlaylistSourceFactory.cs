using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicPlaylistSourceFactory
    {
        PlaylistType SupportedPlaylistTypes { get; }
        Task<IMusicPlaylistSource> CreatePlaylistSourceAsync(string configuration);
    }
}
