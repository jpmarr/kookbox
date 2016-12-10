using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IPlaylistSourceFactory
    {
        PlaylistType SupportedPlaylistTypes { get; }
        Task<IPlaylistSource> CreatePlaylistSourceAsync(string configuration);
    }
}
