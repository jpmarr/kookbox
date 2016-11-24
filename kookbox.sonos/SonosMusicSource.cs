using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;

namespace kookbox.sonos
{
    public class SonosMusicSource : IMusicSource
    {
        public SonosMusicSource()
        {
        }

        public string Name => "Sonos";
        public IMusicPlayer Player { get; }
        public Task<Option<IMusicTrack>> GetTrackAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Option<IMusicAlbum>> GetAlbumAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Option<IMusicArtist>> GetArtistAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IMusicSearchResults> SearchAsync(string searchCriteria)
        {
            throw new System.NotImplementedException();
        }
    }
}
