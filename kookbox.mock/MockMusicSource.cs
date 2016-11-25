using System;
using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    public class MockMusicSource : 
        IMusicSource, 
        IMusicPlaylistSourceFactory
    {
        private readonly IMusicPlayer player;

        public MockMusicSource()
        {
            player = new MockMusicPlayer(this);
        }

        public string Name => "Mock";
        public PlaylistType SupportedPlaylistTypes => PlaylistType.All;
        public IMusicPlayer Player => player;

        public Task<Option<IMusicTrack>> GetTrackAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IMusicAlbum>> GetAlbumAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IMusicArtist>> GetArtistAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IMusicSearchResults> SearchAsync(string searchCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<IMusicPlaylistSource> CreatePlaylistSourceAsync(string configuration)
        {
            return Task.FromResult<IMusicPlaylistSource>(new MockMusicPlaylistSource());
        }
    }
}
