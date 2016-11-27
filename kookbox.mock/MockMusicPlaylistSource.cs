using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    internal class MockMusicPlaylistSource : IMusicPlaylistSource
    {
        private readonly MockMusicSource source;

        public MockMusicPlaylistSource(MockMusicSource source)
        {
            this.source = source;
        }

        public string Name => "Mock Playlist Source";
        public PlaylistType PlaylistType => PlaylistType.Random;
        public bool IsExhausted => false;

        public Task<Option<IMusicTrack>> GetNextTrackAsync()
        {
            return Task.FromResult(Option<IMusicTrack>.Some(source.GetNextRandomTrack()));
        }
    }
}
