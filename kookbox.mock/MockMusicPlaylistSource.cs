using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    internal class MockMusicPlaylistSource : IMusicPlaylistSource
    {
        public string Name => "Mock Playlist Source";
        public PlaylistType PlaylistType => PlaylistType.Random;

        public Task<IMusicTrack> GetNextTrackAsync()
        {
            return Task.FromResult<IMusicTrack>(new MockMusicTrack());
        }
    }
}
