using System.Collections.Generic;
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
        public PlayerCapabilities PlayerCapabilities { get; }
        public IEnumerable<IPlayerDescriptor> AllPlayers { get; }
        public IEnumerable<IPlayerDescriptor> AvailablePlayers { get; }
        public Task<Option<IPlayer>> RequestPlayerAsync(IRoom room, string playerId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Option<IPlayer>> RequestAvailablePlayerAsync(IRoom room)
        {
            throw new System.NotImplementedException();
        }

        public Task<Option<ITrack>> GetTrackAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Option<IAlbum>> GetAlbumAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Option<IArtist>> GetArtistAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ISearchResults> SearchAsync(string searchCriteria)
        {
            throw new System.NotImplementedException();
        }
    }
}
